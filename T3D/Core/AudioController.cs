using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FireAndForgetAudioSample;
using NAudio.Wave;

namespace T3D.Core
{
    public class AudioController
    {
        #region Singleton
        private static AudioController instance;

        private AudioController() { }

        public static AudioController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AudioController();
                }
                return instance;
            }
        }
        #endregion

        public WaveOut AudioPlayer;
        public CachedSound Swap = new CachedSound(@"C:\Users\Asus\Documents\PLUSrotate.mp3");
        public CachedSound Fall = new CachedSound(@"C:\Users\Asus\Documents\PLUSfall.mp3");

    }
}
