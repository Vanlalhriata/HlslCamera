using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Effects;

namespace HlslCamera
{
    public class CustomShaderEffect : ShaderEffect
    {
        public CustomShaderEffect(Uri psUri)
        {
            PixelShader = new PixelShader() { UriSource = psUri };
            UpdateShaderValue(InputProperty);
        }

        public static readonly DependencyProperty InputProperty = RegisterPixelShaderSamplerProperty("Input", typeof(CustomShaderEffect), 0);
        public Brush Input
        {
            get { return (Brush)GetValue(InputProperty); }
            set { SetValue(InputProperty, value); }
        }
    }
}
