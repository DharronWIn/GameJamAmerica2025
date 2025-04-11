using System;
using Fusion;
using UnityEngine;
using DG.Tweening;

namespace Starter.Platformer
{
	/// <summary>
	/// Coin object that can be picked up by player.
	/// </summary>
	[RequireComponent(typeof(Collider))]
	public class Coin : NetworkBehaviour
	{
		[Header("Setup")]
		public float RefreshTime = 4f;

		[Header("References")]
		public Collider Trigger;
		public GameObject VisualRoot;
		public ParticleSystem Particles;

		[Header("Fall Settings")]
		public float FallDelay = 0.2f;
		public float ReactivationDelay = 1f;
		public float FallDuration = 1f;
		public float ReturnDuration = 0.5f;
		public Ease FallEase = Ease.OutBounce;
		public Ease ReturnEase = Ease.InOutQuad;

		public Rigidbody rb;

		public Action CoinCollected;

		public bool IsActive => _activationCooldown.ExpiredOrNotRunning(Runner);

		[Networked]
		private TickTimer _activationCooldown { get; set; }

		[Networked]
		private bool _isActive { get; set; } = true;

		private Vector3 _originalPosition;
		private Tween _currentTween;

		public override void Spawned()
		{
			// Save original platform position, so we can reset position
			// when platform gets reactivated
			_originalPosition = transform.position;
			_isActive = true;

			// Initialiser le délai de chute au départ
			StartFallSequence();
			
			// Initialiser le timer réseau
			_activationCooldown = TickTimer.CreateFromSeconds(Runner, 0f); // Pas de timer au départ
		}

		private void StartFallSequence()
		{
			if (_currentTween != null)
				_currentTween.Kill();

			// Délai avant de tomber, puis animation de chute avec DOTween
			_currentTween = DOTween.Sequence()
				.AppendInterval(FallDelay)
				.Append(transform.DOMove(new Vector3(transform.position.x, 0, transform.position.z), FallDuration).SetEase(FallEase))
				.OnComplete(() => {
					// Fin de l'animation
				});
		}

		private void ReturnToOriginalPosition()
		{
			if (_currentTween != null)
				_currentTween.Kill();

			// Animation de retour à la position d'origine
			_originalPosition = new Vector3(UnityEngine.Random.Range(-10, 10), _originalPosition.y,UnityEngine.Random.Range(-5f, 27f));
			transform.position = _originalPosition;
			//_currentTween = transform.DOMove(_originalPosition, ReturnDuration).SetEase(ReturnEase);
		}

		public void RequestCollect()
		{
			if (IsActive == false)
				return;

			RPC_RequestCollect();
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			if (_currentTween != null)
				_currentTween.Kill();
				
			CoinCollected = null;
		}

		public override void Render()
		{
			bool isActive = IsActive;

			Trigger.enabled = isActive;

			// Show/hide coin visual
			VisualRoot.SetActive(isActive);

			// Start/stop particles emission
			var emission = Particles.emission;
			emission.enabled = isActive;
		}

		public override void FixedUpdateNetwork()
		{
			if (_activationCooldown.Expired(Runner) == true && !_isActive)
			{
				// La pièce est réactivée uniquement après avoir été collectée
				_isActive = true;
				ReturnToOriginalPosition();
				
				// Redémarrer la séquence de chute après un court délai
				DOVirtual.DelayedCall(ReactivationDelay, StartFallSequence);
			}

			// Synchronize isActive state with actual collider state
			Trigger.enabled = _isActive;
		}

		[Rpc(RpcSources.All, RpcTargets.StateAuthority)]
		private void RPC_RequestCollect(RpcInfo info = default)
		{
			if (IsActive == false)
				return;

			// Quand collectée, programmer sa réapparition dans RefreshTime secondes
			_activationCooldown = TickTimer.CreateFromSeconds(Runner, RefreshTime);
			_isActive = false;

			// We are using targeted RPC to send
			// collection message only to the right client (player)
			RPC_CoinCollected(info.Source);
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All)]
		private void RPC_CoinCollected([RpcTarget] PlayerRef playerRef)
		{
			// Arrêter l'animation en cours si elle existe
			if (_currentTween != null)
				_currentTween.Kill();
				
			CoinCollected?.Invoke();
		}
	}
}
