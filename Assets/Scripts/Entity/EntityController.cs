using Mutations.GenericComponents;
using Mutations.Mutations;
using UnityEngine;

namespace Mutations.Entity
{
    [RequireComponent(typeof(Rotator), typeof(MeshRenderer))]
    public class EntityController : MonoBehaviour
    {
        public EntityData data;

        private float _colorCounter = 1;
        private int _colorIndex;


        private ColorMutation _colorMutator;
        private Vector3 _currentSize;

        private int _currentStrength;

        private MeshRenderer _meshRenderer;

        private float _sizeCounter = 5;
        private ScaleMutation _sizeMutator;

        private int _strIndex;
        private StrengthMutation _strMutator;
        
        private float _lvlUpCounter = 3;


        private void Start()
        {
            _meshRenderer = GetComponent<MeshRenderer>();

            //Apply all of the default mutations to the object
            if (data.TryGetMutation(out _colorMutator))
            {
                _colorMutator.ResetToDefault(_meshRenderer);
                _colorIndex = _colorMutator.DefaultIndex;
            }

            if (data.TryGetMutation<RotateMutation>(out var rotMutation))
                rotMutation.ResetToDefault(GetComponent<Rotator>());

            if (data.TryGetMutation(out _sizeMutator))
            {
                _sizeMutator.ResetToDefault(transform);
                _currentSize = _sizeMutator.DefaultValue;
            }

            if (data.TryGetMutation(out _strMutator))
            {
                //this will call IncreaseStrength, setting _CurrentStrength
                _strMutator.ResetToDefault(this);
                _strIndex = _strMutator.DefaultIndex;
            }
        }

        private void Update()
        {
            if (_colorMutator)
            {
                //every second apply the next color mutation to the object
                _colorCounter -= Time.deltaTime;
                if (_colorCounter <= 0)
                {
                    _colorCounter = 1;
                    //Store the resulting index so it can be passed in again
                    _colorIndex = _colorMutator.ApplyNext(_meshRenderer, _colorIndex);
                }
            }

            if (_sizeMutator)
            {
                //every 5 second apply the next scale mutation to the object
                _sizeCounter -= Time.deltaTime;
                if (_sizeCounter <= 0)
                {
                    _sizeCounter = 5;
                    //Store the resulting size so it can be passed in again
                    _currentSize = _sizeMutator.ApplyNext(transform, _currentSize);
                }
            }

            _lvlUpCounter -= Time.deltaTime;
            if (_lvlUpCounter <= 0)
            {
                //Every 3 seconds level up
                _lvlUpCounter = 3;
                LevelUp();
            }
        }

        private void LevelUp()
        {
            Debug.Log($"{gameObject.name}: Level up! New Strength: {_currentStrength}");
            if (_strMutator)
            {
                _strIndex = _strMutator.ApplyNext(this, _strIndex);
                Debug.Log($"{gameObject.name}: New Weapon Weapon {_strMutator.Values[_strIndex]}");

            }
        }

        public void IncreaseStrength(int value)
        {
            _currentStrength = data.strength + value;
        }
    }
}