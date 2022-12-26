using UnityEngine;
using System.Collections.Generic;
using System;
using StudioScor.Utilities;

namespace StudioScor.PlayerSystem
{
    public class PlayerManager : Singleton<PlayerManager>
    {
        #region
        public delegate void SpawnPawnHandler(PlayerManager playerManager, PawnSystem pawn);
        #endregion

        [Header(" [ Default Player Controller ] ")]
        [SerializeField] private ControllerSystem _DefaultPlayerController;

        private ControllerSystem _PlayerController;

        private bool _WasSetup = false;

        public ControllerSystem PlayerController
        {
            get
            {
                if (!_WasSetup)
                    Setup();

                return _PlayerController;
            }
        }
        public PawnSystem PlayerCharacter
        {
            get
            {
                if (!_WasSetup)
                    Setup();

                return _PlayerController.Pawn;
            }
        }

        protected override void Setup()
        {
            if (_WasSetup)
                return;

            _WasSetup = true;

            if (!_PlayerController)
                _PlayerController = Instantiate(_DefaultPlayerController);
        }
    }

}

