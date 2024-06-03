using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;

namespace UnityEngine.XR.Content.Interaction
{
    public class XRLever1 : XRBaseInteractable
    {
        public GameObject prefab; // The prefab to spawn
        private List<GameObject> spawnedObjects = new List<GameObject>(); // List to keep track of spawned objects
        private Dictionary<GameObject, Vector3> initialSpawnPositions = new Dictionary<GameObject, Vector3>(); // Dictionary to store initial spawn positions
        private GameObject currentObject; // The currently active object

        public KeepScore keepScore;

        private float BasketTime;
        private int BasketTimeInt;
        public TextMeshPro timeText;
        public TextMeshPro scoreText;

        private bool Playing;

        const float k_LeverDeadZone = 0.1f; // Prevents rapid switching between on and off states when right in the middle

        [SerializeField]
        [Tooltip("The object that is visually grabbed and manipulated")]
        Transform m_Handle = null;

        [SerializeField]
        [Tooltip("The value of the lever")]
        bool m_Value = false;

        [SerializeField]
        [Tooltip("If enabled, the lever will snap to the value position when released")]
        bool m_LockToValue;

        [SerializeField]
        [Tooltip("Angle of the lever in the 'on' position")]
        [Range(-90.0f, 90.0f)]
        float m_MaxAngle = 90.0f;

        [SerializeField]
        [Tooltip("Angle of the lever in the 'off' position")]
        [Range(-90.0f, 90.0f)]
        float m_MinAngle = -90.0f;

        [SerializeField]
        [Tooltip("Events to trigger when the lever activates")]
        UnityEvent m_OnLeverActivate = new UnityEvent();

        [SerializeField]
        [Tooltip("Events to trigger when the lever deactivates")]
        UnityEvent m_OnLeverDeactivate = new UnityEvent();

        IXRSelectInteractor m_Interactor;

        /// <summary>
        /// The object that is visually grabbed and manipulated
        /// </summary>
        public Transform handle
        {
            get => m_Handle;
            set => m_Handle = value;
        }

        /// <summary>
        /// The value of the lever
        /// </summary>
        public bool value
        {
            get => m_Value;
            set => SetValue(value, true);
        }

        /// <summary>
        /// If enabled, the lever will snap to the value position when released
        /// </summary>
        public bool lockToValue { get; set; }

        /// <summary>
        /// Angle of the lever in the 'on' position
        /// </summary>
        public float maxAngle
        {
            get => m_MaxAngle;
            set => m_MaxAngle = value;
        }

        /// <summary>
        /// Angle of the lever in the 'off' position
        /// </summary>
        public float minAngle
        {
            get => m_MinAngle;
            set => m_MinAngle = value;
        }

        /// <summary>
        /// Events to trigger when the lever activates
        /// </summary>
        public UnityEvent onLeverActivate => m_OnLeverActivate;

        /// <summary>
        /// Events to trigger when the lever deactivates
        /// </summary>
        public UnityEvent onLeverDeactivate => m_OnLeverDeactivate;

        void Start()
        {
            Playing = false;
            SetValue(m_Value, true);

            if (m_Value)
            {
                StartGame();
            }
            else
            {
                RestartTimer();
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            selectEntered.AddListener(StartGrab);
            selectExited.AddListener(EndGrab);
            StartGame();
        }

        protected override void OnDisable()
        {
            selectEntered.RemoveListener(StartGrab);
            selectExited.RemoveListener(EndGrab);
            base.OnDisable();
        }

        void StartGrab(SelectEnterEventArgs args)
        {
            m_Interactor = args.interactorObject;
        }

        void EndGrab(SelectExitEventArgs args)
        {
            SetValue(m_Value, true);
            m_Interactor = null;
        }

        public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
        {
            base.ProcessInteractable(updatePhase);

            if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
            {
                if (isSelected)
                {
                    UpdateValue();
                }
            }
        }

        Vector3 GetLookDirection()
        {
            Vector3 direction = m_Interactor.GetAttachTransform(this).position - m_Handle.position;
            direction = transform.InverseTransformDirection(direction);
            direction.x = 0;

            return direction.normalized;
        }

        void UpdateValue()
        {
            var lookDirection = GetLookDirection();
            var lookAngle = Mathf.Atan2(lookDirection.z, lookDirection.y) * Mathf.Rad2Deg;

            if (m_MinAngle < m_MaxAngle)
                lookAngle = Mathf.Clamp(lookAngle, m_MinAngle, m_MaxAngle);
            else
                lookAngle = Mathf.Clamp(lookAngle, m_MaxAngle, m_MinAngle);

            var maxAngleDistance = Mathf.Abs(m_MaxAngle - lookAngle);
            var minAngleDistance = Mathf.Abs(m_MinAngle - lookAngle);

            if (m_Value)
                maxAngleDistance *= (1.0f - k_LeverDeadZone);
            else
                minAngleDistance *= (1.0f - k_LeverDeadZone);

            var newValue = (maxAngleDistance < minAngleDistance);

            SetHandleAngle(lookAngle);

            SetValue(newValue);
        }

        void SetValue(bool isOn, bool forceRotation = false)
        {
            if (m_Value == isOn)
            {
                if (forceRotation)
                    SetHandleAngle(m_Value ? m_MaxAngle : m_MinAngle);

                return;
            }

            m_Value = isOn;

            if (m_Value)
            {
                m_OnLeverActivate.Invoke();
                RestartTimer();
            }
            else
            {
                m_OnLeverDeactivate.Invoke();
                StartGame();
            }

            if (!isSelected && (m_LockToValue || forceRotation))
                SetHandleAngle(m_Value ? m_MaxAngle : m_MinAngle);
        }

        void SetHandleAngle(float angle)
        {
            if (m_Handle != null)
                m_Handle.localRotation = Quaternion.Euler(angle, 0.0f, 0.0f);
        }

        void OnDrawGizmosSelected()
        {
            var angleStartPoint = transform.position;

            if (m_Handle != null)
                angleStartPoint = m_Handle.position;

            const float k_AngleLength = 0.25f;

            var angleMaxPoint = angleStartPoint + transform.TransformDirection(Quaternion.Euler(m_MaxAngle, 0.0f, 0.0f) * Vector3.up) * k_AngleLength;
            var angleMinPoint = angleStartPoint + transform.TransformDirection(Quaternion.Euler(m_MinAngle, 0.0f, 0.0f) * Vector3.up) * k_AngleLength;

            Gizmos.color = Color.green;
            Gizmos.DrawLine(angleStartPoint, angleMaxPoint);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(angleStartPoint, angleMinPoint);
        }

        void OnValidate()
        {
            SetHandleAngle(m_Value ? m_MaxAngle : m_MinAngle);
        }

        public void StartGame()
        {
            BasketTime = 90;
            Playing = true;
            SpawnPrefab(); // Spawn the first prefab
        }

        void RestartTimer()
        {
            BasketTime = 90;
            Playing = false;
            timeText.text = "90";

            if (keepScore != null)
            {
                keepScore.ResetScore();
            }

            // Destroy the spawned objects
            foreach (var obj in spawnedObjects)
            {
                Destroy(obj);
            }
            spawnedObjects.Clear();
            initialSpawnPositions.Clear();
        }

        void Update()
        {
            if (Playing)
            {
                BasketTime -= Time.deltaTime;

                if (BasketTime <= 0)
                {
                    BasketTime = 0;
                    Playing = false;

                    // Destroy all spawned objects when the time reaches 0
                    foreach (var obj in spawnedObjects)
                    {
                        Destroy(obj);
                    }
                    spawnedObjects.Clear();
                    initialSpawnPositions.Clear();
                }

                BasketTimeInt = Mathf.FloorToInt(BasketTime);
                timeText.text = BasketTimeInt.ToString();

                // Check if the current object is too far from its initial spawn position and spawn a new one if needed
                if (currentObject != null && Vector3.Distance(currentObject.transform.position, initialSpawnPositions[currentObject]) > 0.4f) // Example distance check
                {
                    SpawnPrefab();
                }
            }
        }

        void SpawnPrefab()
        {
            Vector3 spawnPosition = transform.position
                                  + transform.up * -0.4f      // Adjust the height
                                  + transform.forward * 0.2f;  // Adjust the forward distance
            GameObject newObject = Instantiate(prefab, spawnPosition, Quaternion.identity);
            spawnedObjects.Add(newObject);
            initialSpawnPositions[newObject] = spawnPosition;
            currentObject = newObject; // Keep track of the currently active object
        }

    }
}
