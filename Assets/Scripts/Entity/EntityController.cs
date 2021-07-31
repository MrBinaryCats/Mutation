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
                _colorIndex = _colorMutator.ResetToDefaultIndex(_meshRenderer);

            if (data.TryGetMutation<RotateMutation>(out var rotMutation))
                rotMutation.ResetToDefault(GetComponent<Rotator>());

            if (data.TryGetMutation(out _sizeMutator))
                _currentSize = _sizeMutator.ResetToDefault(transform);

            if (data.TryGetMutation(out _strMutator))
                _strIndex = _strMutator.ResetToDefaultIndex(this);//this will call IncreaseStrength, setting _CurrentStrength

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
            if (_strMutator && _strIndex < _strMutator.GetValuesCount()-1)
            {
                _strIndex = _strMutator.ApplyNext(this, _strIndex);
                Debug.Log($"{gameObject.name}: Level up! New Strength: {_currentStrength}, New Weapon Weapon {_strMutator.GetValueAtIndex(_strIndex)}");
            }
            else
            {
                Debug.Log($"{gameObject.name}: Level up!");
            }

        }

        public void IncreaseStrength(int value)
        {
            _currentStrength = data.Strength + value;
        }
    }
}