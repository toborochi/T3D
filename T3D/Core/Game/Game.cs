using NAudio.Wave;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T3D.Core.Shaders;
using Tetris3D;
using QuickFont;
using QuickFont.Configuration;

namespace T3D.Core.Game
{
    public class Game : GameWindow
    {



        bool exit = false;

        public Game() : base(512, 512, new GraphicsMode(32, 24, 0, 4))
        {
            Title = Settings.TITLE;
            Location = Settings.WIN_POSITION;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            WaveFileReader reader = new WaveFileReader(@"C:\Users\Asus\Documents\PLUS.wav");
            LoopStream loop = new LoopStream(reader);
            AudioController.Instance.AudioPlayer = new WaveOut();
            AudioController.Instance.AudioPlayer.Init(loop);
            AudioController.Instance.AudioPlayer.Play();

            GameController.Instance.Initialize();
           

            GL.ClearColor(0.2f, 0.22f, 0.2f, 0);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            
        }

        protected override void OnClosed(EventArgs e)
        {
            ShaderManager.Instance.deleteShaders();
            GameController.Instance.Finish();
            base.OnClosed(e);
            
        }
        
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (Keyboard.GetState().IsKeyDown(Key.Escape) && !exit)
            {
                AudioController.Instance.AudioPlayer.Dispose();
                exit = true;
                Exit();

            }
            GameController.Instance.OnUpdate((float)e.Time);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            GameController.Instance.OnKeyPressed();
        }

        protected override void OnResize(EventArgs e)
        {
         
            base.OnResize(e);
            GL.Viewport(0, 0, Width, Height);
            Settings.CURRENT_WIN_WIDTH = Width;
            Settings.CURRENT_WIN_HEIGHT = Height;
            GameController.Instance.OnResize();

        }

        float Lerp(float firstFloat, float secondFloat, float by)
        {
            return firstFloat * (1 - by) + secondFloat * by;
        }



        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
     
            
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);


            GameController.Instance.OnRender();

 


            GL.Flush();
            SwapBuffers();
        }


    }
}
