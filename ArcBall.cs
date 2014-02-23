using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LD11
{
    /// <summary>
    /// Class holds arcball data
    /// </summary>
    public class ArcBall
    {
        #region Instance Data
        protected Matrix rotation; // Matrix for arc ball's orientation
        protected Matrix translation; // Matrix for arc ball's position
        protected Matrix translationDelta; // Matrix for arc ball's position

        protected int width; // arc ball's window width
        protected int height; // arc ball's window height
        protected Vector2 center;  // center of arc ball 
        protected float radius; // arc ball's radius in screen coords
        protected float radiusTranslation; // arc ball's radius for translating the target

        protected Quaternion downQuat; // Quaternion before button down
        protected Quaternion nowQuat; // Composite quaternion for current drag
        protected bool isDragging; // Whether user is dragging arc ball

        protected Point lastMousePosition; // position of last mouse point
        protected Vector3 downPt; // starting point of rotation arc
        protected Vector3 currentPt; // current point of rotation arc
        #endregion

        #region Simple Properties
        /// <summary>Gets the rotation matrix</summary>
        public Matrix RotationMatrix { get { return rotation = Matrix.CreateFromQuaternion(nowQuat); } }
        /// <summary>Gets the translation matrix</summary>
        public Matrix TranslationMatrix { get { return translation; } }
        /// <summary>Gets the translation delta matrix</summary>
        public Matrix TranslationDeltaMatrix { get { return translationDelta; } }
        /// <summary>Gets the dragging state</summary>
        public bool IsBeingDragged { get { return isDragging; } }
        /// <summary>Gets or sets the current quaternion</summary>
        public Quaternion CurrentQuaternion { get { return nowQuat; } set { nowQuat = value; } }
        #endregion

        /// <summary>
        /// Create new instance of the arcball class
        /// </summary>
        public ArcBall()
        {
            Reset();
            downPt = Vector3.Zero;
            currentPt = Vector3.Zero;

            SetWindow(800, 600);
        }

        /// <summary>
        /// Resets the arcball
        /// </summary>
        public void Reset()
        {
            msLast = Mouse.GetState();

            downQuat = Quaternion.Identity;
            nowQuat = Quaternion.Identity;
            rotation = Matrix.Identity;
            translation = Matrix.Identity;
            translationDelta = Matrix.Identity;
            isDragging = false;
            radius = 1.0f;
            radiusTranslation = 1.0f;
        }

        /// <summary>
        /// Convert a screen point to a vector
        /// </summary>
        public Vector3 ScreenToVector(Matrix view, Matrix projection, Matrix world, GraphicsDevice device, float screenPointX, float screenPointY)
        {
            float x = (screenPointX - width / 2.0f) / (radius * width / 2.0f);
            float y = (screenPointY - height / 2.0f) / (radius * height / 2.0f);
            float z = 0.0f;
            float mag = (x * x) + (y * y);

            if (mag > 1.0f) {
                float scale = 1.0f / (float)Math.Sqrt(mag);
                x *= scale;
                y *= scale;
            } else
                z = (float)Math.Sqrt(1.0f - mag);

            return new Vector3(x, y, z);
            /*
            Vector3 a = new Vector3(x, y, z);
            Vector3 b = Vector3.Transform(a, projection);

            return b;*/
        }

        /// <summary>
        /// Set window paramters
        /// </summary>
        public void SetWindow(int w, int h, float r)
        {
            width = w; height = h; radius = r;
            center = new Vector2(w / 2.0f, h / 2.0f);
        }
        public void SetWindow(int w, int h)
        {
            SetWindow(w, h, 0.9f); // default radius
        }

        /// <summary>
        /// Computes a quaternion from ball points
        /// </summary>
        public static Quaternion QuaternionFromBallPoints(Vector3 from, Vector3 to)
        {
            float dot = Vector3.Dot(from, to);
            Vector3 part = Vector3.Cross(from, to);
            return new Quaternion(part.X, part.Y, part.Z, dot);
        }

        /// <summary>
        /// Begin the arcball 'dragging'
        /// </summary>
        public void OnBegin(Matrix view, Matrix projection, Matrix world, GraphicsDevice device, int x, int y)
        {
            isDragging = true;
            downQuat = nowQuat;

            downPt = ScreenToVector(view, projection, world, device, (float)x, (float)y);
        }
        /// <summary>
        /// The arcball is 'moving'
        /// </summary>
        public void OnMove(Matrix view, Matrix projection, Matrix world, GraphicsDevice device, int x, int y)
        {
            if (isDragging) {
                currentPt = ScreenToVector(view, projection, world, device, (float)x, (float)y);
                nowQuat = downQuat * QuaternionFromBallPoints(downPt, currentPt);
            }
        }
        /// <summary>
        /// Done dragging the arcball
        /// </summary>
        public void OnEnd()
        {
            isDragging = false;
        }

        MouseState msLast;

        public void Update(Matrix view, Matrix projection, Matrix world, GraphicsDevice device)
        {
            MouseState ms = Mouse.GetState();

            int mouseX = ms.X;
            int mouseY = ms.Y;

            if (ms.RightButton == ButtonState.Pressed) {
                OnMove(view, projection, world, device, mouseX, mouseY);

                if (msLast.RightButton == ButtonState.Released) {
                    OnBegin(view, projection, world, device, ms.X, ms.Y);
                    msLast = ms;
                    return;
                }
            }
            if (ms.RightButton == ButtonState.Released && msLast.RightButton == ButtonState.Pressed) {
                OnEnd();
                msLast = ms;
                return;
            }

            msLast = ms;
        }
    }
}
