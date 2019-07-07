using FireAndForgetAudioSample;
using OpenTK;
using OpenTK.Input;
using QuickFont;
using QuickFont.Configuration;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using T3D.Core.Game;
using Tetris3D;


namespace T3D.Core
{
    public class GameController
    {
        #region Singleton
        private static GameController instance;

        private GameController() { }

        public static GameController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameController();
                }
                return instance;
            }
        }
        #endregion


        Tetromino t;
        Container c;
        public Camera cam = new Camera();
        float time = 0;
        float altura = 12;
        bool GameStatus = true;

        private QFont _myFont;
        private QFontDrawing _drawing;
        private Matrix4 _projectionMatrix;

        List<Tetromino.Tipo> Tipo = new List<Tetromino.Tipo>()
        {
            Tetromino.Tipo.I,
            Tetromino.Tipo.L,
            Tetromino.Tipo.O,
            Tetromino.Tipo.S,
            Tetromino.Tipo.T,
            Tetromino.Tipo.Z
        };

        public void Finish()
        {
            _myFont.Dispose();
            _drawing.Dispose();
        }

        public void OnResize()
        {
            _projectionMatrix = Matrix4.CreateOrthographicOffCenter(0, Settings.CURRENT_WIN_WIDTH, 0, Settings.CURRENT_WIN_HEIGHT
                , -1.0f, 1.0f);
        }

        Random Random = new Random(int.MaxValue);
        float posx = 0, posy = 0, dist = 10;
        float intervalo = 0.5f;
        public int puntuacion = 0;

        public void Initialize()
        {
           
            c = new Container(8, 8, 14);
            t = new Tetromino();
            t.Position = new Vector3(4, 12, 4);

            _myFont = new QFont(@"C:\Users\Asus\Desktop\arial.ttf", 20, new QFontBuilderConfiguration(true));
            _drawing = new QFontDrawing();
            _projectionMatrix = Matrix4.CreateOrthographicOffCenter(0, Settings.CURRENT_WIN_WIDTH, 0, Settings.CURRENT_WIN_HEIGHT
                , -1.0f, 1.0f);


        }

        public void CameraInput()
        {
            if (Keyboard.GetState().IsKeyDown(Key.Right))
            {
                posy -= 0.25f;
            }
            if (Keyboard.GetState().IsKeyDown(Key.Left))
            {
                posy += 0.25f;
            }

            if (Keyboard.GetState().IsKeyDown(Key.ControlLeft))
            {
                dist = Lerp(dist, 15, 0.5f);
            }

            if (Keyboard.GetState().IsKeyDown(Key.ShiftLeft))
            {
                dist = Lerp(dist, 9f, 0.5f);
            }
        }

        public void Input()
        {

            if (Keyboard.GetState().IsKeyDown(Key.Space))
            {
                intervalo = 0.1f;
            }

            if (Keyboard.GetState().IsKeyUp(Key.Space))
            {
                intervalo = 0.5f;
            }



            if (Keyboard.GetState().IsKeyDown(Key.Up))
            {
                posx -= 0.25f;
            }
            if (Keyboard.GetState().IsKeyDown(Key.Down))
            {
                posx += 0.25f;
            }

            CameraInput();

        }

        public void OnUpdate(float delta)
        {

            if (GameStatus)
            {
                time += delta;
                Input();
            }
           

            posx = (float)MathHelper.Clamp(posx, -Math.PI / 2 + 0.01f, Math.PI / 2 - 0.01f);
            posy = (float)MathHelper.Clamp(posy, -Math.PI + 0.01f, Math.PI - 0.01f);


            cam.distance = dist;
            cam.updateCameraPosition(posx, posy, 0);

            //t.Position = new Vector3(t.Position.X, altura, t.Position.Z);
            if(GameStatus)
            {
            if (time > intervalo)
            {
                if (c.encaja(t.CurrentPiece, t.Position + new Vector3(0, -1, 0)))
                {
                    altura--;
                    t.Position = new Vector3(t.Position.X, altura, t.Position.Z);
                }
                else
                {
                    if (c.encaja(t.CurrentPiece, t.Position))
                    {

                        AudioPlaybackEngine.Instance.PlaySound(AudioController.Instance.Fall);
                        c.encajar(t.CurrentPiece, t.Position);
                        puntuacion += c.checkLine();

                        //tetromino.Puntos = Piezas[Random.Next(0, Piezas.Count)];
                        altura = 12;

                        if (c.encaja(t.CurrentPiece, new Vector3(4, 12, 4)))
                        {
                            t.setPiece(Tipo[Random.Next(0, Tipo.Count)]);
                            //posicionX = posicionZ = 4;
                            t.Position = new Vector3(4, 12, 4);
                            //tetromino.transform.Position = new Vector3(posicionX, altura, posicionZ);
                            //tetromino.Color = Colores[Random.Next(0, Colores.Count)];
                        }

                    }
                    else
                    {
                        GameStatus = false;
                    }
                }

                time = 0;
             }

                t.OnUpdate(cam);
                c.Update(cam);

            }

          

          

        }
        

        public void OnRender()
        {
            if (GameStatus) { 
           
                t.Draw();
                c.Draw();
                
            }
            _drawing.DrawingPrimitives.Clear();
            _drawing.ProjectionMatrix = _projectionMatrix;
            _drawing.Print(_myFont, "SCORE: " + puntuacion.ToString(), new Vector3(Settings.CURRENT_WIN_WIDTH/2-100, Settings.CURRENT_WIN_HEIGHT*5/6, 0), QFontAlignment.Left);
            _drawing.RefreshBuffers();
            _drawing.Draw();

        }

        public void OnKeyPressed()
        {

            if (Keyboard.GetState().IsKeyDown(Key.Z))
            {
                if (c.encaja(t.RotarTetrominoX(t.CurrentPiece), t.Position))
                {
                    t.RotateX();
                    AudioPlaybackEngine.Instance.PlaySound(AudioController.Instance.Swap);
                }

            }

            if (Keyboard.GetState().IsKeyDown(Key.X))
            {
                if (c.encaja(t.RotarTetrominoY(t.CurrentPiece), t.Position))
                {
                    t.RotateY();
                    AudioPlaybackEngine.Instance.PlaySound(AudioController.Instance.Swap);
                }
            }

            if (Keyboard.GetState().IsKeyDown(Key.C))
            {
                if (c.encaja(t.RotarTetrominoZ(t.CurrentPiece), t.Position))
                {
                    t.RotateZ();
                    AudioPlaybackEngine.Instance.PlaySound(AudioController.Instance.Swap);

                }
            }

            if (Keyboard.GetState().IsKeyDown(Key.D))
            {
                if (c.encaja(t.CurrentPiece, t.Position + new Vector3(1, 0, 0)))
                {
                    t.Position += new Vector3(1, 0, 0);
                }
            }
            if (Keyboard.GetState().IsKeyDown(Key.A))
            {
                if (c.encaja(t.CurrentPiece, t.Position + new Vector3(-1, 0, 0)))
                {
                    t.Position += new Vector3(-1, 0, 0);
                }
            }

            if (Keyboard.GetState().IsKeyDown(Key.W))
            {
                if (c.encaja(t.CurrentPiece, t.Position + new Vector3(0, 0, -1)))
                {
                    t.Position += new Vector3(0, 0, -1);
                }


            }
            if (Keyboard.GetState().IsKeyDown(Key.S))
            {
                if (c.encaja(t.CurrentPiece, t.Position + new Vector3(0, 0, 1)))
                {
                    t.Position += new Vector3(0, 0, 1);
                }
            }

        }

        float Lerp(float firstFloat, float secondFloat, float by)
        {
            return firstFloat * (1 - by) + secondFloat * by;
        }

    }
}
