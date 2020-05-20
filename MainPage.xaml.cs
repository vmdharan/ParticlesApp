using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ParticlesApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        // Random number generator
        public static Random rand = new Random();

        // Define the particle structure
        public struct Particles
        {
            public Ellipse ellipse;
            public double newLeft;
            public double newTop;
            public double hDir;
            public double vDir;
            public SolidColorBrush col1;
        }

        private const int MAXWIDTH = 1280;
        private const int MAXHEIGHT = 720;

        private Particles[] particles;          // Array of particles
        private const int maxParticles = 64;    // Number of particles to draw
        private float radius = 32.0f;           // Particle size

        // Store the width and height limits
        private int wComponent;
        private int hComponent;

        public MainPage()
        {
            this.InitializeComponent();

            InitParticles();
            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }

        private void CompositionTarget_Rendering(object sender, object e)
        {
            // Loop over all the particles.
            for (int i = 0; i < maxParticles; i++)
            {
                // Update the horizontal position.
                particles[i].newLeft += particles[i].hDir;

                // If outside the bounds, then invert the direction of motion.
                if (particles[i].newLeft <= 0)
                {
                    particles[i].hDir = 1;
                }
                else if (particles[i].newLeft >= wComponent)
                {
                    particles[i].hDir = -1;
                }

                // Update the vertical position.
                particles[i].newTop += particles[i].vDir;

                // If outside the bounds, then invert the direction of motion.
                if (particles[i].newTop <= 0)
                {
                    particles[i].vDir = 1;
                }
                else if (particles[i].newTop >= hComponent)
                {
                    particles[i].vDir = -1;
                }

                // Update the position of the ellipse on the Canvas.
                particles[i].ellipse.SetValue(Canvas.LeftProperty, particles[i].newLeft);
                particles[i].ellipse.SetValue(Canvas.TopProperty, particles[i].newTop);
            }
        }

        public void InitParticles()
        {
            particles = new Particles[maxParticles];

            // Create an array of random colours.
            SolidColorBrush[] scb = new SolidColorBrush[10];

            scb[0] = new SolidColorBrush(Color.FromArgb(255, 124, 212, 85));
            scb[1] = new SolidColorBrush(Color.FromArgb(255, 124, 85, 212));
            scb[2] = new SolidColorBrush(Color.FromArgb(255, 85, 124, 212));
            scb[3] = new SolidColorBrush(Color.FromArgb(255, 212, 124, 85));
            scb[4] = new SolidColorBrush(Color.FromArgb(255, 212, 85, 124));
            scb[5] = new SolidColorBrush(Color.FromArgb(255, 85, 212, 124));
            scb[6] = new SolidColorBrush(Color.FromArgb(255, 24, 21, 85));
            scb[7] = new SolidColorBrush(Color.FromArgb(255, 24, 85, 21));
            scb[8] = new SolidColorBrush(Color.FromArgb(255, 85, 21, 24));
            scb[9] = new SolidColorBrush(Color.FromArgb(255, 85, 85, 124));

            // Set the x and y bounds.
            wComponent = MAXWIDTH - (int)radius;
            hComponent = MAXHEIGHT - (int)radius;

            // Loop over all the particles and draw them on the canvas.
            for (int i = 0; i < maxParticles; i++)
            {
                particles[i].newLeft = rand.Next(0, wComponent);
                particles[i].newTop = rand.Next(0, hComponent);
                particles[i].col1 = scb[rand.Next(0, 9)];

                particles[i].hDir = rand.Next(1, 2) * -1;
                particles[i].vDir = rand.Next(1, 2) * -1;

                particles[i].ellipse = new Ellipse();
                particles[i].ellipse.Width = radius;
                particles[i].ellipse.Height = radius;
                particles[i].ellipse.Fill = particles[i].col1;

                canvas.Children.Add(particles[i].ellipse);
                Canvas.SetLeft(particles[i].ellipse, particles[i].newLeft);
                Canvas.SetTop(particles[i].ellipse, particles[i].newTop);
            }
        }

        private void MainPage_Unloaded(object sender, RoutedEventArgs e)
        {
            CompositionTarget.Rendering -= CompositionTarget_Rendering;
        }
    }
}
