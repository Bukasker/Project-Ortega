﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // Importujemy, aby korzystać z EventSystem
using Mapbox.Unity.Map;
using TMPro;

namespace Mapbox.Examples
{
    public class AstronautMouseController : MonoBehaviour
    {
        [Header("Character")]
        [SerializeField]
        GameObject character;
        [SerializeField]
        float characterSpeed;
        [SerializeField]
        Animator characterAnimator;

        [Header("References")]
        [SerializeField]
        AstronautDirections directions;
        [SerializeField]
        Transform startPoint;
        [SerializeField]
        Transform endPoint;
        [SerializeField]
        AbstractMap map;
        [SerializeField]
        GameObject rayPlane;
        [SerializeField]
        Transform _movementEndPoint;

        [SerializeField]
        LayerMask layerMask;

        Ray ray;
        RaycastHit hit;
        LayerMask raycastPlane;
        float clicktime;
        bool moving;
        bool characterDisabled;

        void Start()
        {
            characterAnimator = GetComponentInChildren<Animator>();
            if (!Application.isEditor)
            {
                this.enabled = false;
                return;
            }
        }

        void Update()
        {

            if (characterDisabled)
                return;

            previousPos = transform.position;
            CamControl();

            // Ignoruj kliknięcia, jeśli wskaźnik jest nad UI
            if (IsPointerOverUI())
            {
                return;
            }

            bool click = false;

            if (Input.GetMouseButtonDown(0))
            {
                clicktime = Time.time;
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (Time.time - clicktime < 0.15f)
                {
                    click = true;
                }
            }

            if (click)
            {
                ray = cam.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
                {
                    startPoint.position = transform.localPosition;
                    endPoint.position = hit.point;
                    MovementEndpointControl(hit.point, true);

                    directions.Query(GetPositions, startPoint, endPoint, map);
                }
            }
        }

        #region Character : Movement
        List<Vector3> futurePositions;
        bool interruption;
        void GetPositions(List<Vector3> vecs)
        {
            futurePositions = vecs;

            if (futurePositions != null && moving)
            {
                interruption = true;
            }
            if (!moving)
            {
                interruption = false;
                MoveToNextPlace();
            }
        }

        Vector3 nextPos;
        void MoveToNextPlace()
        {
            if (futurePositions.Count > 0)
            {
                nextPos = futurePositions[0];
                futurePositions.Remove(nextPos);

                moving = true;
                characterAnimator.SetBool("IsWalking", true);
                StartCoroutine(MoveTo());
            }
            else if (futurePositions.Count <= 0)
            {
                moving = false;
                characterAnimator.SetBool("IsWalking", false);
            }
        }

        Vector3 prevPos;
        IEnumerator MoveTo()
        {
            prevPos = transform.localPosition;

            float time = CalculateTime();
            float t = 0;

            StartCoroutine(LookAtNextPos());

            while (t < 1 && !interruption)
            {
                t += Time.deltaTime / time;

                transform.localPosition = Vector3.Lerp(prevPos, nextPos, t);

                yield return null;
            }

            interruption = false;
            MoveToNextPlace();
        }

        float CalculateTime()
        {
            float timeToMove = 0;

            timeToMove = Vector3.Distance(prevPos, nextPos) / characterSpeed;

            return timeToMove;
        }
        #endregion

        #region Character : Rotation
        IEnumerator LookAtNextPos()
        {
            Quaternion neededRotation = Quaternion.LookRotation(nextPos - character.transform.position);
            Quaternion thisRotation = character.transform.localRotation;

            float t = 0;
            while (t < 1.0f)
            {
                t += Time.deltaTime / 0.25f;
                var rotationValue = Quaternion.Slerp(thisRotation, neededRotation, t);
                character.transform.rotation = Quaternion.Euler(0, rotationValue.eulerAngles.y, 0);
                yield return null;
            }
        }
        #endregion

        #region CameraControl
        [Header("CameraSettings")]
        [SerializeField]
        Camera cam;
        Vector3 previousPos = Vector3.zero;
        Vector3 deltaPos = Vector3.zero;

        void CamControl()
        {
            var transformChr = character.GetComponent<Transform>();
            Vector3 targetPos = new Vector3(transformChr.position.x, cam.transform.position.y, transformChr.position.z-12f);
            cam.transform.position = Vector3.Lerp(cam.transform.position, targetPos, Time.deltaTime * 5f);
        }
        #endregion

        #region Utility
        public void DisableCharacter()
        {
            characterDisabled = true;
            moving = false;
            StopAllCoroutines();
            character.SetActive(false);
        }

        public void EnableCharacter()
        {
            characterDisabled = false;
            character.SetActive(true);
        }

        public void LayerChangeOn()
        {
            Debug.Log("OPEN");
        }

        public void LayerChangeOff()
        {
            Debug.Log("CLOSE");
        }

        void MovementEndpointControl(Vector3 pos, bool active)
        {
            _movementEndPoint.position = new Vector3(pos.x, 0.2f, pos.z);
            _movementEndPoint.gameObject.SetActive(active);
        }
        #endregion

        #region UI Handling
        private bool IsPointerOverUI()
        {
            // Sprawdza, czy wskaźnik myszy jest nad UI (dla PC)
            if (EventSystem.current.IsPointerOverGameObject())
                return true;

            // Sprawdza dla dotyku (dla urządzeń mobilnych)
            if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
            {
                if (EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
                    return true;
            }

            return false;
        }
        #endregion
    }
}
