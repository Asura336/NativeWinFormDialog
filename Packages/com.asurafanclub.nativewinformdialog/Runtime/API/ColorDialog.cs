using System;
using System.Runtime.InteropServices;
using DialogLib.Data;
using DialogLib.Foundations;
using UnityEngine;

namespace DialogLib
{
    public class ColorDialog
    {
        public bool AllowFullOpen { get; set; } = true;
        public bool AnyColor { get; set; } = false;
        public Color32 Color { get; set; } = new Color32(0xFF, 0xFF, 0xFF, 0xFF);
        public Color32[] CustomColors { get; set; } = null;
        public bool FullOpen { get; set; } = false;
        public bool SolidColorOnly { get; set; } = false;

        public void Reset()
        {
            AllowFullOpen = true;
            AnyColor = false;
            Color = new Color32(0xFF, 0xFF, 0xFF, 0xFF);
            CustomColors = null;
            FullOpen = false;
            SolidColorOnly = false;
        }

        public unsafe DialogResult ShowDialog()
        {
            GetParams(out var @params);
            DialogResult result = default;

            var paramPnt = &@params;
            //--------
            // CustomColors 内部实现长度固定，考虑栈分配？
            const int fixedCustomColorBuffLen = 16;
            uint* _fixedCustumColorsBuff = stackalloc uint[fixedCustomColorBuffLen];
            new Span<uint>(_fixedCustumColorsBuff, fixedCustomColorBuffLen).Fill(0x00FFFFFF);
            var _cc = &paramPnt->CustomColors;
            _cc->buffer = (ColorARGB*)_fixedCustumColorsBuff;
            _cc->length = fixedCustomColorBuffLen;
            _cc->count = fixedCustomColorBuffLen;
            if (CustomColors != null && CustomColors.Length > 0)
            {
                int count = Math.Min(fixedCustomColorBuffLen, CustomColors.Length);
                for (int i = 0; i < count; i++)
                {
                    var tc = CustomColors[i];
                    // HACK:
                    // 在本机代码库传递自定义颜色缓冲区都靠直接复制内存
                    // 但是回传的颜色结果正确，传入的颜色 R 通道和 B 通道对换了
                    // 在这里手动处理一下
                    _cc->buffer[i] = new ColorARGB(0, tc.b, tc.g, tc.r);
                }
            }

            result = ShowColorDialog(paramPnt);

            Color = (Color32)paramPnt->Color;
            ref var _customColors = ref paramPnt->CustomColors;
            if (_customColors.count != 0)
            {
                var _copyCustomColors = new Color32[_customColors.count];
                var src = _customColors.buffer;
                for (int i = 0; i < _customColors.count; i++)
                {
                    // 传回的自定义颜色高位（alpha）总是 0，这个颜色选择器不支持透明度，手动设置为不透明颜色
                    var c = src[i];
                    c.a = 0xFF;
                    _copyCustomColors[i] = c;
                }
                CustomColors = _copyCustomColors;
            }

            return result;
        }

        unsafe void GetParams(out ColorDialogParams @params)
        {
            @params = new ColorDialogParams
            {
                AllowFullOpen = AllowFullOpen,
                AnyColor = AnyColor,
                Color = Color,
                //CustomColors = _customColors,
                SolidColorOnly = SolidColorOnly,
            };
        }

        [DllImport("DialogLib", CharSet = CharSet.Unicode)]
        static extern unsafe DialogResult ShowColorDialog([In, Out] ColorDialogParams* Param);
    }
}
