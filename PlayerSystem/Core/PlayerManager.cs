using UnityEngine;
using System.Collections.Generic;
using System;

namespace StudioScor.PlayerSystem
{
    public class PlayerManager : MonoBehaviour
    {
        #region
        public delegate void SpawnPawnHandler(PlayerManager playerManager, PawnSystem pawn);
        #endregion

        private static PlayerManager _Instance = null;

        [Header(" [ Default Player Controller ] ")]
        [SerializeField] private ControllerSystem _DefaultPlayerController;

        [Header(" [ Instance ] ")]
        [SerializeField] private ControllerSystem _PlayerController;
        public PawnSystem PlayerCharacter => PlayerController.Pawn;

        [Header(" [ Pawns ] ")]
        [SerializeField] private List<PawnSystem> _AiPawns;

        public IReadOnlyList<PawnSystem> AiPawns => _AiPawns;

        public event SpawnPawnHandler OnAddedPawn;
        public event SpawnPawnHandler OnRemovedPawn;
        public static PlayerManager Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = FindObjectOfType<PlayerManager>();
                }

                return _Instance;
            }
        }

        public ControllerSystem PlayerController
        {
            get
            {
                if (_PlayerController == null)
                {
                    _PlayerController = Instantiate(_DefaultPlayerController);
                }

                return _PlayerController;
            }
        }

        void Awake()
        {
            if (_Instance == null)
            {
                _Instance = this;
            }
        }

        public void AddPawn(PawnSystem pawnSystem)
        {
            _AiPawns.Add(pawnSystem);

            OnAddedPawn?.Invoke(this, pawnSystem);

        }
        public void RemovePawn(PawnSystem pawnSystem)
        {
            _AiPawns.Add(pawnSystem);

            OnRemovedPawn?.Invoke(this, pawnSystem);
        }

        public void SetPlayerPawn(PawnSystem newPlayerPawn)
        {
            if (PlayerCharacter != null)
            {
                var pawn = PlayerCharacter;

                pawn.UnPossess(PlayerController);

                if(pawn != null)
                {
                    AddPawn(pawn);
                }
            }

            if(newPlayerPawn != null)
            {
                newPlayerPawn.OnPossess(PlayerController);

                _AiPawns.Remove(newPlayerPawn);
            }
        }
    }

}

