
///	
///	Button.cs
///
///		 by Dave Peckham
///		 August 2002
///		 Irvine, California
///	
///	A button that emulates the Apple Aqua user interface guidelines.
///	This button grows horizontally to accomodate its label. Button
///	height is fixed.
///	

using System;
using System.Reflection;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;


namespace Wildgrape.Aqua.Controls
{
	[Description( "Aqua Button Control" )]
	[Designer(typeof (Wildgrape.Aqua.Controls.ButtonDesigner))]
	public class Button : System.Windows.Forms.Button 
	{
		#region Class Constants

		protected static int ButtonDefaultWidth = 80;

		// Set this to the height of your source bitmaps
		protected static int ButtonHeight = 30;

		// If your source bitmaps have shadows, set this 
		// to the shadow size so DrawText can position the 
		// label appears centered on the label
		protected static int ButtonShadowOffset = 5;

		// These settings approximate the pulse effect
		// of buttons on Mac OS X
		protected static int PulseInterval = 70;
		protected static float PulseGammaMax = 1.8f;
		protected static float PulseGammaMin = 0.7f;
		protected static float PulseGammaShift = 0.2f;
		protected static float PulseGammaReductionThreshold = 0.2f;
		protected static float PulseGammaShiftReduction = 0.5f;

		#endregion


		#region Member Variables

		// Appearance
		protected bool	pulse = false;
		protected bool	sizeToLabel = true;

		// Pulsing
		protected Timer timer;
		protected float gamma, gammaShift;

		// Mouse tracking
		protected Point ptMousePosition;
		protected bool	mousePressed;

		// Images used to draw the button
		protected Image imgLeft, imgFill, imgRight;

		// Rectangles to position images on the button face
		protected Rectangle rcLeft, rcRight;

		// Matrices for transparency transformation
		protected ImageAttributes iaDefault, iaNormal;
		protected ColorMatrix cmDefault, cmNormal;

		#endregion


		#region Constructors and Initializers

		public Button( ) 
		{
			InitializeComponent(  );
			SetStyle( ControlStyles.StandardClick, true );
		}

		private void InitializeComponent( )
		{
		}

		#endregion


		#region Properties

		[Description( "Determines whether the button pulses. Note that only the default button can pulse." )]
		[Category( "Appearance" )]
		[DefaultValue( false )]
		public bool Pulse
		{
			get { return pulse; }
			set { pulse = value; }
		}

		[Description( "Determines whether the button should automatically size to fit the label" )]
		[Category( "Layout" )]
		[DefaultValue( true )]
		public bool SizeToLabel
		{
			get { return sizeToLabel; }
			set 
			{ 
				sizeToLabel = value;
				OnTextChanged( EventArgs.Empty ); 
			}
		}

		#endregion


		#region Property overrides

		/* AquaButton has a fixed height */
		protected override Size DefaultSize
		{
			get	{return new Size( Button.ButtonDefaultWidth, 
					 Button.ButtonHeight ); }
		}

		/* Shadow Control.Width to make it browsable */
		[Description( "See also: SizeToLabel" )]
		[Category( "Layout" )]
		[Browsable( true )]
		public new int Width 
		{
			get { return base.Width; }
			set { base.Width = value; }
		}

		/* Shadow Control.Height to make it browsable and read only */
		[Description( "Aqua buttons have a fixed height" )]
		[Category( "Layout" )]
		[Browsable( true )]
		[ReadOnly( true )]
		public new int Height {	get { return base.Height; }	}

		#endregion


		#region Method overrides

		protected override void OnCreateControl()
		{
			LoadImages();
			InitializeGraphics();
		}

		protected override void OnTextChanged( EventArgs e )
		{
			if (sizeToLabel) 
			{
				Graphics g = this.CreateGraphics( );
				SizeF sizeF = g.MeasureString( Text, Font );
				Width = imgLeft.Width + (int)sizeF.Width + imgRight.Width;
				g.Dispose();
			}
			Invalidate( );
			Update( );
			base.OnTextChanged( e );
		}

		protected override void OnPaint( PaintEventArgs e )
		{
			Graphics g = e.Graphics;
			g.Clear(Parent.BackColor);
			Draw( g );
		}

		protected override void OnGotFocus( EventArgs e )
		{
			base.OnGotFocus( e );

			if (Pulse)
				StartPulsing();
		}
		
		protected override void OnLostFocus( EventArgs e )
		{
			base.OnLostFocus( e );

			if (Pulse)
				StopPulsing();
		}
		
		protected override void OnMouseDown( MouseEventArgs e )
		{
			base.OnMouseDown( e );

			if( e.Button == MouseButtons.Left ) 
			{
				mousePressed = true;

				ptMousePosition.X = e.X;
				ptMousePosition.Y = e.Y;

				StopPulsing( );
			}
		}

		protected override void OnMouseMove( MouseEventArgs e )
		{
			// Buttons receives MouseMove events when the
			// mouse enters or leaves the client area.

			base.OnMouseMove( e );

			if( mousePressed && ( e.Button & MouseButtons.Left ) != 0 )
			{
				ptMousePosition.X = e.X;
				ptMousePosition.Y = e.Y;
			} 
		}

		protected override void OnMouseUp( MouseEventArgs e )
		{
			base.OnMouseUp( e );

			if( mousePressed ) 
			{
				mousePressed = false;

				StartPulsing( );

				Invalidate();
				Update( );
			}
		}

		protected override void OnKeyPress( KeyPressEventArgs e )
		{
			base.OnKeyPress( e );

			if( mousePressed && e.KeyChar == '\x001B' )  // Escape
			{
				mousePressed = false;

				StartPulsing( );

				Invalidate();
				Update( );
			}
		}

		#endregion


		#region Implementation

		protected virtual void LoadImages ()
		{
			imgLeft = new Bitmap( GetType(), "Button.left.png" );
			imgRight = new Bitmap( GetType(), "Button.right.png" );
			imgFill = new Bitmap( GetType(), "Button.fill.png" );
		}

		protected virtual void InitializeGraphics ()
		{
			// Rectangles for placing images relative to the client rectangle
			rcLeft = new Rectangle( 0, 0, imgLeft.Width, imgLeft.Height );
			rcRight = new Rectangle( 0, 0, imgRight.Width, imgRight.Height );

			// Image attributes used to lighten default buttons

			cmDefault = new ColorMatrix();
			cmDefault.Matrix33 = 0.5f;  // reduce opacity by 50%

			iaDefault = new ImageAttributes();
			iaDefault.SetColorMatrix( cmDefault, ColorMatrixFlag.Default, 
				ColorAdjustType.Bitmap );
			
			// Image attributes that lighten and desaturate normal buttons
	
			cmNormal = new ColorMatrix();

			// desaturate the image
			cmNormal.Matrix00 = 1/3f;
			cmNormal.Matrix01 = 1/3f;
			cmNormal.Matrix02 = 1/3f;
			cmNormal.Matrix10 = 1/3f;
			cmNormal.Matrix11 = 1/3f;
			cmNormal.Matrix12 = 1/3f;
			cmNormal.Matrix20 = 1/3f;
			cmNormal.Matrix21 = 1/3f;
			cmNormal.Matrix22 = 1/3f;
			cmNormal.Matrix33 = 0.5f;  // reduce opacity by 50%

			iaNormal = new ImageAttributes();
			iaNormal.SetColorMatrix( cmNormal, ColorMatrixFlag.Default, 
				ColorAdjustType.Bitmap );
		}

		protected virtual void StartPulsing ()
		{
			if ( Focused && Pulse && !this.DesignModeDetected( ) )
			{
				timer = new Timer( );
				timer.Interval = Button.PulseInterval;
				timer.Tick += new EventHandler( TimerOnTick );
				gamma = Button.PulseGammaMax;
				gammaShift = -Button.PulseGammaShift;
				timer.Start();
			}
		}

		protected virtual void StopPulsing ()
		{
			if ( timer != null )
			{
				iaDefault.SetGamma( 1.0f, ColorAdjustType.Bitmap );
				timer.Stop();
			}
		}

		protected virtual void Draw( Graphics g )
		{
			DrawButton( g );
			DrawText( g );
		}

		protected virtual void DrawButton( Graphics g )
		{
			// Update our destination rectangles
			rcRight.X = this.Width - imgRight.Width;

			if ( mousePressed )
			{
				if ( ClientRectangle.Contains( ptMousePosition.X, ptMousePosition.Y ) )
					DrawButtonState( g, iaDefault );
				else
					DrawButtonState( g, iaNormal );
			}
			else if ( IsDefault )
				DrawButtonState( g, iaDefault );
			else
				DrawButtonState( g, iaNormal );
		}

		protected virtual void DrawButtonState (Graphics g, ImageAttributes ia)
		{
			TextureBrush tb;

			// Draw the left and right endcaps
			g.DrawImage( imgLeft, rcLeft, 0, 0, 
				imgLeft.Width, imgLeft.Height, GraphicsUnit.Pixel, ia );

			g.DrawImage( imgRight, rcRight, 0, 0, 
				imgRight.Width, imgRight.Height, GraphicsUnit.Pixel, ia );

			// Draw the middle
			tb = new TextureBrush( imgFill, 
				new Rectangle( 0, 0, imgFill.Width, imgFill.Height ), ia );
			tb.WrapMode = WrapMode.Tile;

			g.FillRectangle ( tb, imgLeft.Width, 0, 
				this.Width - (imgLeft.Width + imgRight.Width), 
				imgFill.Height);

			tb.Dispose( );
		}

		protected virtual void DrawText( Graphics g ) 
		{
			RectangleF layoutRect = 
				new RectangleF( 0, 0, this.Width, 
					this.Height - Button.ButtonShadowOffset );

			int LabelShadowOffset = 1;

			StringFormat fmt	= new StringFormat( );
			fmt.Alignment		= StringAlignment.Center;
			fmt.LineAlignment	= StringAlignment.Center;

			// Draw the shadow below the label
			layoutRect.Offset( 0, LabelShadowOffset );
			SolidBrush textShadowBrush	= new SolidBrush( Color.Gray );
			g.DrawString( Text, Font, textShadowBrush, layoutRect, fmt );
			textShadowBrush.Dispose( );

			// and the label itself
			layoutRect.Offset( 0, -LabelShadowOffset );
			SolidBrush brushFiller	= new SolidBrush( Color.Black );
			g.DrawString( Text, Font, brushFiller, layoutRect, fmt );
			brushFiller.Dispose( );
		}

		protected virtual void TimerOnTick( object obj, EventArgs e)
		{
			// set the new gamma level
			if ((gamma - Button.PulseGammaMin < Button.PulseGammaReductionThreshold ) || 
				(Button.PulseGammaMax - gamma < Button.PulseGammaReductionThreshold ))
				gamma += gammaShift * Button.PulseGammaShiftReduction;
			else
				gamma += gammaShift;

			if ( gamma <= Button.PulseGammaMin || gamma >= Button.PulseGammaMax )
				gammaShift = -gammaShift;

			iaDefault.SetGamma( gamma, ColorAdjustType.Bitmap );

			Invalidate( );
			Update( );
		}

		protected virtual bool DesignModeDetected()
		{
			// base.DesignMode always returns false, so try this workaround
			IDesignerHost host = 
				(IDesignerHost) this.GetService( typeof( IDesignerHost ) );

			return ( host != null );
		}

		#endregion
	}
}


