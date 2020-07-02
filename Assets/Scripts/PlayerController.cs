﻿/*
 * Copyright 2002-2020 the original author or authors.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      https://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using UnityEngine;

namespace Examproject {
    /// <summary>
    /// player controller.
    /// @author h.adachi
    /// </summary>
    public class PlayerController : GamepadMaper {

        ///////////////////////////////////////////////////////////////////////////////////////////
        // References

        [SerializeField]
        private float rotationalSpeed = 5.0f;

        ///////////////////////////////////////////////////////////////////////////////////////////
        // Fields

        private DoFixedUpdate doFixedUpdate;

        private float speed;

        ///////////////////////////////////////////////////////////////////////////////////////////
        // update Methods

        // Awake is called when the script instance is being loaded.
        void Awake() {
            doFixedUpdate = DoFixedUpdate.GetInstance();
        }

        // Start is called before the first frame update
        new void Start() {
        }

        // Update is called once per frame
        new void Update() {
            base.Update();
            if (upButton.isPressed) {
                doFixedUpdate.walk = true;
            } else if (downButton.isPressed) {
                doFixedUpdate.backward = true;
            }

            var _ADJUST1 = 12.0f;
            var _axis = rightButton.isPressed ? 1 : leftButton.isPressed ? -1 : 0;
            transform.Rotate(0, _axis * (rotationalSpeed * Time.deltaTime) * _ADJUST1, 0); // rotate
        }

        // FixedUpdate is called just before each physics update.
        void FixedUpdate() {
            var _rb = transform.GetComponent<Rigidbody>(); // Rigidbody should be only used in FixedUpdate.
            speed = _rb.velocity.magnitude; // get speed.

            var _ADJUST1 = 12.0f;
            if (doFixedUpdate.walk) {
                _rb.AddFor​​ce(transform.forward * _ADJUST1, ForceMode.Acceleration); // move forward.
            } else if (doFixedUpdate.backward) {
                _rb.AddFor​​ce(-transform.forward * _ADJUST1, ForceMode.Acceleration); // move backward.
            }

            doFixedUpdate.ResetMotion(); // initialize physical behavior flag.
        }

        #region DoFixedUpdate

        /// <summary>
        /// structure for the FixedUpdate() method.
        /// </summary>
        protected struct DoFixedUpdate {

            ///////////////////////////////////////////////////////////////////////////////////////
            // Fields

            private bool _idol;
            private bool _walk;
            private bool _backward;

            public bool idol { get => _idol; set => _idol = value; }
            public bool walk { get => _walk; set => _walk = value; }
            public bool backward { get => _backward; set => _backward = value; }

            ///////////////////////////////////////////////////////////////////////////////////////
            // Constructor

            /// <summary>
            /// returns an initialized instance.
            /// </summary>
            public static DoFixedUpdate GetInstance() {
                var _instance = new DoFixedUpdate();
                _instance.ResetMotion();
                return _instance;
            }

            ///////////////////////////////////////////////////////////////////////////////////////
            // public Methods

            /// <summary>
            /// initialization of all fields.
            /// </summary>
            public void ResetMotion() {
                _idol = false;
                _walk = false;
                _backward = false;
            }
        }

        #endregion

    }

}
