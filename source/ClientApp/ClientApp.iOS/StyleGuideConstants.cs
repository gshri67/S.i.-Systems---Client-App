using UIKit;

namespace ClientApp.iOS
{
    public static class StyleGuideConstants
    {
        //See: https://devfacto.slack.com/files/stephen/F03JGRKHX/si_styleguide.pdf
        public static readonly UIColor RedUiColor = new UIColor(0.96875f, 0.2265625f, 0.24609375f, 1); //#f83a3f
        public static readonly UIColor GreenUiColor = new UIColor(0.70703125f, 0.8359375f, 0.65234375f, 1); //#3bb44a
        public static readonly UIColor LightGreenUiColor = new UIColor(0.23046875f, 0.703125f, 0.2890625f, 1); //#b5d6a7
        public static readonly UIColor LightGrayUiColor = new UIColor(0.859375f, 0.859375f, 0.859375f, 1); //#eaeaea
        public static readonly UIColor DarkGrayUiColor = new UIColor(0.15234375f, 0.15234375f, 0.15234375f, 1); //#272727

        public const string DateSeperator = "\u2192";
    }
}