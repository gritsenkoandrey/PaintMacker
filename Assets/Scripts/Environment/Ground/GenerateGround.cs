using BaseMonoBehaviour;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Environment.Ground
{
    public sealed class GenerateGround : BaseComponent
    {
        [SerializeField] private Ground _ground;

        [SerializeField] private int _width;
        [SerializeField] private int _height;

        public int Width => _width;
        public int Height => _height;

        [Button(ButtonSizes.Large)]
        public void Generate()
        {
            float scale = 1f / _ground.transform.localScale.x;
            
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    Ground ground = Instantiate(_ground, 
                        new Vector3(x/scale, 0f, y/scale), _ground.transform.rotation, transform);

                    ground.name = $"Ground_{x}:{y}";
                    ground.Pixel.x = x;
                    ground.Pixel.y = y;
                    
                    if (x == 0 || x == _width - 1 || y == 0 || y == _height - 1)
                    {
                        ground.Pixel.index = 1;
                    }
                    else
                    {
                        ground.Pixel.index = 0;
                    }
                }
            }
            
            Debug.Log($"Generate {_width * _height} ground");
        }
    }
}