using Microsoft.EntityFrameworkCore;

namespace BL
{
    public class Ticket
    {
        public static ML.Result ReadData(string file, FileInfo pathFile)
        {
            ML.Result result = new ML.Result();

            using (DL.TicketsContext db = new DL.TicketsContext())
            {
                using (var transaction = db.Database.BeginTransaction())
                {

                    try
                    {

                        string[] data = file.Replace("-", "").Replace(">", "").Replace(" ", "").Trim().Split("|");

                        #region GetDate

                        //ONLY DATE
                        string[] date = new string[data[2].Length];

                        for (int i = 0; i <= data[2].Length - 1; i++)
                        {
                            int j = 1;
                            date[i] = data[2].Substring(i, j);
                            j++;
                        }

                        var entireDate = date[0] + date[1] + date[2] + date[3] + "-" + date[4] + date[5] + "-" + date[6] + date[7];


                        //ONLY TIME
                        string[] time = new string[data[3].Length];

                        for (int i = 0; i <= data[3].Length - 1; i++)
                        {
                            int j = 1;
                            time[i] = data[3].Substring(i, j);
                            j++;
                        }

                        var entireTime = time[0] + time[1] + ":" + time[2] + time[3] + ":" + time[4] + time[5];

                        DateTime fechaHoraTicket = Convert.ToDateTime(entireDate).Add(TimeSpan.Parse(entireTime)); //Convertir y juntar dos cadenas a tipo DATETIME

                        #endregion

                        //DateTime fechaHoraTicket;

                        ML.Ticket ticket = new ML.Ticket
                        {
                            IdTienda = data[0],
                            IdRegistradora = data[1],
                            FechaHoraTicket = entireDate + " " + entireTime,
                            //FechaHoraTicket = fechaHoraTicket,
                            //FechaTicket = DateTime.TryParseExact(data[2], "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out fechaHoraTicket)
                            //              ? fechaHoraTicket : DateTime.MinValue
                            NoTicket = int.Parse(data[4]),
                            ImporteImpuesto = decimal.Parse(data[5]),
                            Total = decimal.Parse(data[6]),
                            FechaCreacion = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")
                        };


                        var query = db.Database.ExecuteSqlRaw($"AddTicket '{ticket.IdTienda}', '{ticket.IdRegistradora}', '{ticket.FechaHoraTicket}', {ticket.NoTicket} , {ticket.ImporteImpuesto}, {ticket.Total}, '{ticket.FechaCreacion}'");

                        #region INSERT CON SQLPARAMETERS
                        //var query = db.Database.ExecuteSqlRaw("AddTicket @IdTienda, @IdRegistradora, @FechaHoraTicket, @Ticket, @Impuesto, @Total, @FechaHoraCreacion",
                        //     new SqlParameter("IdTienda", ticket.IdTienda),
                        //    new SqlParameter("IdRegistradora", ticket.IdRegistradora),
                        //    new SqlParameter("FechaHoraTicket", ticket.FechaHoraTicket),
                        //    new SqlParameter("Ticket", ticket.NoTicket),
                        //    new SqlParameter("Impuesto", ticket.ImporteImpuesto),
                        //    new SqlParameter("Total", ticket.Total),
                        //    new SqlParameter("FechaHoraCreacion", ticket.FechaCreacion));
                        #endregion


                        if (query > 0)
                        {
                            result.Correct = true;

                            MoveFile(pathFile, result.Correct);

                            transaction.Commit();
                        }
                        else
                        {
                            result.Correct = false;

                            MoveFile(pathFile, result.Correct);

                            transaction.Rollback();

                        }
                    }

                    catch (Exception ex)
                    {
                        if (ex.Message != "Error al intentar mover el archivo")
                        {
                            MoveFile(pathFile, false);
                        }

                        result.Correct = false;
                        result.ErrorMessage = ex.Message;
                        result.Ex = ex;

                        transaction.Rollback();
                    }
                }
            }
            return result;
        }


        public static void MoveFile(FileInfo pathFile, bool flag)
        {
            string destinationDirectory = @"C:\Users\Alien 18\Documents\BRodriguezReasignacion\Tickets\Procesados";

            try
            {

                // Ensure that the target does not exist.
                if (File.Exists(destinationDirectory + "\\" + pathFile.Name))
                {
                    File.Delete(destinationDirectory + "\\" + pathFile.Name);
                }

                if (flag)
                {
                    // Move the file.
                    File.Move(pathFile.ToString(), destinationDirectory + "\\" + pathFile.Name); //solo usa 2 parametros, ruta inicial del archivo, y ruta a donde se movera con posibilidad de renombrmiento
                }
                else
                {
                    // Move the file.
                    File.Move(pathFile.ToString(), destinationDirectory + "\\" + pathFile.Name + "_error"); //mover archivo con nuevo nombre indicando el "error"
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error al intentar mover el archivo");
            }
        }

    }
}