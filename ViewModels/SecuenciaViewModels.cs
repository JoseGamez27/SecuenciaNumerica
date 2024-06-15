using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SecuenciaNumerica.ViewModels
{
    public class SecuenciaViewModels : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public byte Aciertos { get; set; }
        public byte Errores { get; set; }
        private int[]? Secuencia;
        public string SecuenciaMostrar { get; set; } = "";
        public string Incremento { get; set; } = "";
        private int NumAdivinar1 { get; set; }
        public int Respuesta1 { get; set; } = 0;   
        private int NumAdivinar2 { get; set; }
        public int Respuesta2 { get; set; } = 0;

        public ICommand IniciarJuegoCommand {  get; set; }
        public ICommand VerificarRespuestaCommand { get; set; }
        public ICommand VolverInicioCommand { get; set; }
        Random r = new();
        public SecuenciaViewModels()
        {
            GenerarSecuencia();
            IniciarJuegoCommand = new Command(IniciarJuego);
            VerificarRespuestaCommand = new Command(VerificarRespuesta);
            VolverInicioCommand = new Command(VolverInicio);
        }    
        void IniciarJuego()
        {
            Aciertos = 0;
            Errores = 0;
            GenerarSecuencia();
            Shell.Current.GoToAsync("//juego");
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }
        void GenerarSecuencia()
        {
            int longitudsecuencia = r.Next(4, 10);
            int valorinicial = r.Next(1, 501);
            int incremento = r.Next(2, 101);
            Incremento = incremento.ToString();

            int[] secuencia = new int[longitudsecuencia];
            secuencia[0] = valorinicial;

            for (int i = 1; i < secuencia.Length; i++)
            {
                secuencia[i] = valorinicial += incremento;
            }
            int indice1 = r.Next(0, secuencia.Length - 1);
            NumAdivinar1 = secuencia[indice1];
            int indice2 = r.Next(0, secuencia.Length);
            NumAdivinar2 = secuencia[indice2];

            while ((indice1 == indice2) && indice1 > indice2) 
            {
                indice2 = r.Next(0, secuencia.Length);
            }
            Secuencia = secuencia;

            string[] secuenciaclon = new string[longitudsecuencia];

            for (int i = 0; i < secuenciaclon.Length; i++)
            {
                secuenciaclon[i] = secuencia[i].ToString();
            }
            secuenciaclon[indice1] = "?";
            secuenciaclon[indice2] = "?";
            SecuenciaMostrar = string.Join(" , ", secuenciaclon);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }
        void VerificarRespuesta()
        {
            if ((Respuesta1 == NumAdivinar1) && (Respuesta2 == NumAdivinar2))
            {
                Aciertos++;
                if (Aciertos == 20)
                {
                    Shell.Current.GoToAsync("//resultados");
                }
                GenerarSecuencia();
            }
            else
            {
                Errores++;
                if (Errores == 3)
                {
                    Shell.Current.GoToAsync("//resultados");
                }
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }
        void VolverInicio()
        {
            Shell.Current.GoToAsync("//inicio");
        }
    }
}
