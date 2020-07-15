using System.ComponentModel;
using System.Threading.Tasks;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.V4.Content;
using Android.Views;
using LinC.Controls;
using LinC.Droid.Helper;
using LinC.Droid.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Material.Android;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomPicker), typeof(CustomPickerRenderer))]
namespace LinC.Droid.Renderer
{
    public class CustomPickerRenderer : MaterialPickerRenderer
    {
        public CustomPickerRenderer(Context context) : base(context)
        {
            MaterialHelper.AndroidContext = context;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            if (e?.NewElement != null)
            {
                SetControl();
                Control.Background =  AddPickerStyles();
            }
        }

        public LayerDrawable AddPickerStyles()
        {
            ShapeDrawable border = new ShapeDrawable();
            border.Paint.Color = Android.Graphics.Color.Transparent;
            border.SetPadding(0, 0, 40, 0);
            border.Paint.SetStyle(Paint.Style.Stroke);

            Drawable[] layers = { border,  GetDrawable() };
            LayerDrawable layerDrawable = new LayerDrawable(layers);
            layerDrawable.SetLayerInset(0, 0, 0, 0, 0);

            return layerDrawable;
        }

        private BitmapDrawable GetDrawable()
        {
            Drawable drawable = ContextCompat.GetDrawable(MaterialHelper.AndroidContext, Resource.Drawable.DropdownArrow);
            

            //int resID = Context.Resources.GetIdentifier(imagePath, "drawable", this.Context.PackageName);
            //var drawable = ContextCompat.GetDrawable(this.Context, resID);

            
            
            var bitmap = ((BitmapDrawable)drawable).Bitmap;

            var result = new BitmapDrawable(Resources, Bitmap.CreateScaledBitmap(bitmap, 45, 25, true));
            result.Gravity = Android.Views.GravityFlags.Right;

            return result;
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            SetControl();
            Control.Background = AddPickerStyles();
        }

        private void SetControl()
        {
            if (Control == null)
            {
                return;
            }

            //Control.SetGravity(GravityFlags.CenterVertical);
            //Control.SetPadding(10, 0, 0, 0);
            
            Control.EditText.Background = null;
            Control.EditText.SetBackgroundColor(Android.Graphics.Color.Transparent);

            Control.EditText.SetTextColor(Android.Graphics.Color.Black);
            Control.EditText.PaintFlags = PaintFlags.LinearText;
        }
    }
}
