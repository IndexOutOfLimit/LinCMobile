using System;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Util;
using Cognizant.Hackathon.Shared.Mobile.Shared.Controls;
using LinC.Droid.Helper;
using LinC.Droid.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Material.Android;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomEditor), typeof(CustomEditorRenderer))]
namespace LinC.Droid.Renderer
{
	public class CustomEditorRenderer : MaterialEditorRenderer
	{
		public CustomEditorRenderer(Context context) : base(context)
		{
			MaterialHelper.AndroidContext = context;
		}
		
		protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
		{

			base.OnElementChanged(e);
			if (e.NewElement != null)
			{
				var view = (CustomEditor)Element;
				if (view.IsCurvedCornersEnabled)
				{
					// creating gradient drawable for the curved background  
					Control.EditText.Background = null;
					Control.EditText.SetBackgroundColor(Android.Graphics.Color.Transparent);

					Control.EditText.SetTextColor(global::Android.Graphics.Color.Black);
				}

				if (!view.IsBorderEnabled)
				{
					GradientDrawable gd = new GradientDrawable();
					gd.SetColor(global::Android.Graphics.Color.Transparent);
					Control.SetBackground(gd);
				}

				// Set padding for the internal text from border  
				//Control.SetPadding(
				//	(int)DpToPixels(this.Context, Convert.ToSingle(10)), 10,
				//	(int)DpToPixels(this.Context, Convert.ToSingle(3)), 5);
			}
		}
		public static float DpToPixels(Context context, float valueInDp)
		{
			DisplayMetrics metrics = context.Resources.DisplayMetrics;
			return TypedValue.ApplyDimension(ComplexUnitType.Dip, valueInDp, metrics);
		}
	}
}