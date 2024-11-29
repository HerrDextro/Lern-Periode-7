using System;
using System.Data;
using System.Media;
using System.Runtime.InteropServices;
using System.Threading;
using Graphic_Renderer;

namespace Graphic_Renderer
{
    public class MusicPlayer
    {

        string audioSource;
        
        public MusicPlayer(string filepath)
        {
            audioSource = filepath;
        }

        private void Play()
        {
            string audioPath = @"..\..\..\Tetris\audio\tetris.wav"; ; // Use a .wav file.
            SoundPlayer player = new SoundPlayer(audioPath);

            // Loop the audio file
            while (true)
            {
                player.PlaySync(); // PlaySync blocks the thread until the file finishes playing
            }

        }


    }
}