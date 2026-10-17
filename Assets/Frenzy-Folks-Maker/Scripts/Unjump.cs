using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Readers;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;

namespace Frenzy_Folks_Maker.Scripts
{
    [AddComponentMenu("XR/Locomotion/Unjump Provider", 11)]
    public class Unjump : LocomotionProvider
    {
        [SerializeField]
        [Tooltip("How far the XR Origin moves downward when the input is pressed.")]
        private float m_DescentDistance = 1.25f;

        [SerializeField]
        [Tooltip("How long the downward movement takes, in seconds.")]
        private float m_DescentDuration = 0.35f;

        [SerializeField]
        [Tooltip("Input used to start the downward movement.")]
        private XRInputButtonReader m_UnjumpInput = new XRInputButtonReader("Jump");

        private readonly XROriginMovement m_Transformation = new XROriginMovement();
        private bool m_IsDescending;
        private bool m_HasUnjumped;
        private float m_DescentTimer;

        public float descentDistance
        {
            get => m_DescentDistance;
            set => m_DescentDistance = Mathf.Max(0f, value);
        }

        public float descentDuration
        {
            get => m_DescentDuration;
            set => m_DescentDuration = Mathf.Max(0f, value);
        }

        public XRInputButtonReader unjumpInput
        {
            get => m_UnjumpInput;
            set => XRInputReaderUtility.SetInputProperty(ref m_UnjumpInput, value, this);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            m_UnjumpInput.EnableDirectActionIfModeUsed();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            m_UnjumpInput.DisableDirectActionIfModeUsed();
        }

        private void Update()
        {
            if (m_HasUnjumped && m_UnjumpInput.ReadWasCompletedThisFrame())
                m_HasUnjumped = false;

            if (!m_HasUnjumped && m_UnjumpInput.ReadIsPerformed())
            {
                m_HasUnjumped = true;
                m_IsDescending = true;
                m_DescentTimer = 0f;
            }

            if (!m_IsDescending)
                return;

            if (locomotionState != LocomotionState.Moving &&
                !TryStartLocomotionImmediately())
            {
                return;
            }

            float duration = Mathf.Max(0.0001f, m_DescentDuration);
            float remainingDistance = m_DescentDistance -
                m_DescentDistance * Mathf.Clamp01(m_DescentTimer / duration);
            float distanceThisFrame = Mathf.Min(
                remainingDistance,
                m_DescentDistance * Time.deltaTime / duration);

            m_Transformation.motion = Vector3.down * distanceThisFrame;
            TryQueueTransformation(m_Transformation);

            m_DescentTimer += Time.deltaTime;
            if (m_DescentTimer >= duration || remainingDistance <= distanceThisFrame)
            {
                m_IsDescending = false;
                TryEndLocomotion();
            }
        }
    }
}
