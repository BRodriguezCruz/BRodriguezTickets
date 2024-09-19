using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Runtime.ConstrainedExecution;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

namespace TicketWindowsService
{
    public partial class Service1 : ServiceBase
    {
        private readonly HttpClient _cliente;
        private readonly EventLog _eventLog; //Event Logo para registrar los ventos en un Log
        private System.Timers.Timer _timer;
        public Service1()
        {
            InitializeComponent();
            _cliente = new HttpClient();
            _eventLog = new EventLog(); //se configura el event LOg para usarse en mi sevicio

            #region Creacion LOG (Ejemplo)
            //Creamos un objecto EventLog:

            //this.ServiceName = "MyService";
            //this.EventLog = new System.Diagnostics.EventLog();
            //this.EventLog.Source = this.ServiceName;
            //this.EventLog.Log = "Application";


            //You also need to create a source, if the above source doesn't exist:

            //((ISupportInitialize)(this.EventLog)).BeginInit();
            //            if (!EventLog.SourceExists(this.EventLog.Source))
            //            {
            //                EventLog.CreateEventSource(this.EventLog.Source, this.EventLog.Log);
            //            }
            //((ISupportInitialize)(this.EventLog)).EndInit();
            //and then simply use it:
            //this.EventLog.WriteEntry("My Eventlog message.", EventLogEntryType.Information);

            #endregion
            try
            {
                if (!EventLog.SourceExists("TicketWinServiceSource"))
                {
                    EventLog.CreateEventSource("TicketWinServiceSource", "TicketWinServiceLog");
                }
                _eventLog.Source = "TicketWinServiceSource";
                _eventLog.Log = "TicketWinServiceLog";
            }
            catch (Exception ex)
                {
                Console.WriteLine($"Error al crear los eventos: {ex.Message}");
            }
        }

        protected override void OnStart(string[] args) //onStart = el temporizador dispara el evento Elapsed c/minuto.
        {
            _timer = new System.Timers.Timer();
            _timer.Interval = 60000; // Intervalo en milisegundos (60000 ms = 1 minuto)
            _timer.Elapsed += async (sender, e) => await OnTimedEvent();
            #region Descripcion Elapsed
            //El evento Elapsed se configura para llamar a un método async que a su vez llama a ConsumeApi.Debido a que el método Elapsed no puede ser async, 
            //    utilizamos una expresión lambda async para invocar OnTimedEvent.
            #endregion
            _timer.AutoReset = true; // Repetir la acción
            _timer.Enabled = true;   // Inicia el temporizador
        }

        protected override void OnStop()
        {
            _timer.Stop();
            _timer.Dispose();
        }

        //Este método es async y llama a ConsumeApi.Dado que OnTimedEvent es async, puede manejar operaciones asíncrona
        private async Task OnTimedEvent()
        {
            await ConsumeApi();
        }

        public async Task ConsumeApi()
        {
            try
            {
                var response = await _cliente.GetAsync("http://localhost:5267/api/Ticket/oldest");
                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {
                    _eventLog.WriteEntry($"Éxito al hacer petición al API: {response}", EventLogEntryType.Information);
                }
                else
                {
                    _eventLog.WriteEntry($"Error al hacer la petición al API: {response}", EventLogEntryType.Error);
                }
            }
            catch (Exception ex)
            {
                _eventLog.WriteEntry($"Error al consumir la API: {ex.Message}", EventLogEntryType.Error);
            }
        }
    }
}
