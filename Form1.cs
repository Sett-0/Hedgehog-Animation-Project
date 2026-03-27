using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using NAudio.Wave;

namespace Practice_10
{
	public partial class Form1 : Form
	{
		int x1 = 50, x2 = 50;     // Координаты ежиков

        bool hedgehog1 = false;			// Направление движения 1 ежика: вправо или влево
		bool hedgehog2 = false;			// Направление движения 2 ежика: вправо или влево

        private WaveOutEvent audioPlayer;       // Плеер
        private AudioFileReader musicFile;      // Считыватель MP3 файла
        private bool isPaused = false;          // Контроллер воспроизведения

        int angle;

		Thread t1;		// Ежик 1
		Thread t2;      // Ежик 2
		Thread t3;		// Солнце
		Thread t4;      // Музыка

		const string dirPath = "../../Files/";

		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			this.MaximumSize = new Size(500, 600);
			this.MinimumSize = new Size(500, 600);


			// Цвет фона
			this.BackColor = Color.AliceBlue;


			// Убираем мерцание при перерисовке
			this.DoubleBuffered = true;


			// Подключаем событие Paint
			 this.Paint += new PaintEventHandler(Form1_Paint);


			// Кнопка включения музыки
			Button b1 = new Button();
			b1.Location = new Point(10, 10);
			b1.Size = new Size(80, 40);
			b1.Text = "Play music";

			b1.Click += new EventHandler(b1_Click);

			Controls.Add(b1);


			// Кнопка остановки музыки
			Button b2 = new Button();
			b2.Location = new Point(10, 55);
			b2.Size = new Size(80, 40);
			b2.Text = "Pause music";

			b2.Click += new EventHandler(b2_Click);

			Controls.Add(b2);


			// Ежик 1
			t1 = new Thread(new ThreadStart(Run1));
			t1.Start();

			// Ежик 2
			t2 = new Thread(new ThreadStart(Run2));
			t2.Start();

			// Солнце
			t3 = new Thread(new ThreadStart(Run3));
			t3.Start();

			// Музыка
			t4 = new Thread(new ThreadStart(Run4));
			t4.Start();
		}


		// Фон
		Image bg = Image.FromFile(dirPath + "bg_hedgehog_home.png");

		Image sun = Image.FromFile(dirPath + "sun.png");

		// Ежик: влево или вправо
		Image img1 = Image.FromFile(dirPath + "hedgehog1.png");
		Image img2 = Image.FromFile(dirPath + "hedgehog2.png");

		private void Form1_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.DrawImage(bg, 0, 0, this.Width, this.Height);
			

			// В зависимости от направления движения ежика: влево или вправо
			if (hedgehog1 == false)
				e.Graphics.DrawImage(img1, x1, 420, 100, 100);
			else
				e.Graphics.DrawImage(img2, x1, 420, 100, 100);


			// В зависимости от направления движения ежика: влево или вправо
			if (hedgehog2 == false)
				e.Graphics.DrawImage(img1, x2, 420, 100, 100);
			else
				e.Graphics.DrawImage(img2, x2, 420, 100, 100);


			// Движение солнца

			// Смещаем начало координат
			e.Graphics.TranslateTransform(370, 100);

			// Вращение начала координат
			e.Graphics.RotateTransform(angle);

			// Рисуем солнце в новой с.к.
			e.Graphics.DrawImage(sun, -60, -60, 120, 120);
		}


		public void Run1()
		{
			int speed1 = 5;
			while (true)
			{
				x1 += speed1;
				if (x1 > 350 || x1 < 50)
				{
					speed1 = -speed1;
				}
				hedgehog1 = speed1 < 0;

				// Перерисовка формы
				this.Invalidate();
				Thread.Sleep(20);	// Определяет скорость ежика
			}
		}


		public void Run2()
		{
            int speed2 = 7;
            while (true)
            {
                x2 += speed2;
                if (x2 > 350 || x2 < 50)
                {
                    speed2 = -speed2;
                }
                hedgehog2 = speed2 < 0;

                // Перерисовка формы
                this.Invalidate();
                Thread.Sleep(20);   // Определяет скорость ежика
            }
        }


		public void Run3()
		{
			// Пересчёт угла поворота
			while(true)
			{
				angle++;

				this.Invalidate();
				Thread.Sleep(20);
			}
		}


		public void Run4()
		{
            musicFile = new AudioFileReader(dirPath + "Ezhik-rezinovy.mp3");
            audioPlayer = new WaveOutEvent();
            audioPlayer.Init(musicFile);
		}

		private void b1_Click(object sender, EventArgs e)
		{
            if (audioPlayer != null)
            {
                if (isPaused)
                {
                    audioPlayer.Play(); // Resume
                    isPaused = false;
                }
                else
                {
                    musicFile.Position = 0; // Start from beginning
                    audioPlayer.Play();
                }
            }
        }


		private void b2_Click(object sender, EventArgs e)
		{
            if (audioPlayer != null && audioPlayer.PlaybackState == PlaybackState.Playing)
            {
                audioPlayer.Pause();
                isPaused = true;
            }
        }
	}
}
