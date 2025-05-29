using GYMHECTORAPI.DataAccess;
using GYMHECTORAPI.Entities.Usuario;
using System.Data;
using System.Net;
using System.Text.Json;
using System.Text;

namespace GYMHECTORAPI.Bussiness
{
    public class UsuarioBO : IUsuarioBO
    {
        private readonly IUsuarioDO _usuarioDO;
        private readonly ILogger<UsuarioBO> _log;
        public UsuarioBO(IUsuarioDO usuarioDO, ILogger<UsuarioBO> log)
        {
            _usuarioDO = usuarioDO;
            _log = log;
        }

        public async Task<ListarMaestrosResponse> ListarMaestros(int idUsuario)
        {
            try
            {
                var response = new ListarMaestrosResponse();

                response.data = new DataMaestros();
                response.data.datosUsuario = await _usuarioDO.ObtenerDatosUsuario(idUsuario);
                var listaHorarios = await _usuarioDO.ListarHorariosRegistrados(idUsuario);

                if (listaHorarios != null && listaHorarios.Count > 0)
                {
                    response.data.ListaHorariosRegistrados = listaHorarios.Select(x => new ListaHorariosRegistrados()
                    {
                        idHorarioRegistrado = x.idHorarioRegistrado,
                        NombreClase = x.NombreClase,
                        FechaInicio = x.FechaInicio,
                        FechaFin = x.FechaFin,
                        NombreProfesor = x.NombreProfesor
                    }).ToList();
                }

                if (response.data.datosUsuario != null)
                {
                    response.codigoRes = HttpStatusCode.OK;
                    response.mensajeRes = "Maestros obtenidos correctamente.";
                    response.data = response.data;
                }
                else
                {
                    response.codigoRes = HttpStatusCode.BadRequest;
                    response.mensajeRes = "No se pudo obtener los maestros.";
                }

                return response;
            }
            catch (Exception ex)
            {
                _log.LogError("{ListarMaestros} Error: " + ex.ToString());
                return new ListarMaestrosResponse
                {
                    codigoRes = HttpStatusCode.InternalServerError,
                    mensajeRes = "Error interno al obtener los maestros. " + ex.ToString()
                };
            }

        }

        public async Task<ListarHorariosGenerales> ListarHorariosGenerales(int idUsuario)
        {
            try
            {
                var response = new ListarHorariosGenerales();

                response.data = new DataHorarios();
                var listaHorarios = await _usuarioDO.ListaHorariosGenerales(idUsuario);

                if (listaHorarios != null && listaHorarios.Count > 0)
                {
                    var dataOrdenada = listaHorarios
                            .GroupBy(x => x.nombreClase)
                            .Select(g => new ListaHorariosGeneral
                            {
                                nombreClase = g.Key,
                                horariosClase = g.Select(h => new ListaHorariosClase
                                {
                                    idHorario = h.idHorario,
                                    FechaInicio = h.FechaInicio,
                                    FechaFin = h.FechaFin,
                                    NombreProfesor = h.NombreProfesor,
                                    FlagDiponible = h.FlagDiponible,
                                    Descripcion = h.Descripcion
                                }).ToList()
                            })
                            .OrderBy(x => x.nombreClase)
                            .ToList();

                    response.codigoRes = HttpStatusCode.OK;
                    response.mensajeRes = "Horarios obtenidos correctamente.";
                    response.data.ListaHorarios = dataOrdenada;
                }
                else
                {
                    response.codigoRes = HttpStatusCode.BadRequest;
                    response.mensajeRes = "No se pudo obtener los horarios.";
                }

                return response;
            }
            catch (Exception ex)
            {
                _log.LogError("{ListarHorariosGenerales} Error: " + ex.ToString());
                return new ListarHorariosGenerales
                {
                    codigoRes = HttpStatusCode.InternalServerError,
                    mensajeRes = "Error interno al obtener los horarios. " + ex.ToString()
                };
            }

        }

        public async Task<RegistrarReservaResponse> registrarReserva(int idUsuarioEdita, RegistraReservaRequest req)
        {
            var idUsuario = idUsuarioEdita;
            if (idUsuario > 0 && req.intIdHorario > 0)
            {
                return await _usuarioDO.registrarReserva(idUsuario, req.intIdHorario, req.intFlagImpedimento);
            }
            else
            {
                return new RegistrarReservaResponse()
                {
                    codigoRes = HttpStatusCode.BadRequest,
                    mensajeRes = "El usuario o el horario no es válido."
                };
            }

        }
        public async Task<HorarioPredicciones> CapacidadHorariosIA(int idUsuario)
        {
            try
            {
                var response = new HorarioPredicciones();

                var listaSuscripciones = await _usuarioDO.CapacidadHorariosIA(idUsuario);
                var listaAsistencias = await _usuarioDO.AsistenciasIA(idUsuario);
                var listaReserva = await _usuarioDO.ReservasIA(idUsuario);

                if (listaSuscripciones != null || listaAsistencias != null || listaReserva != null)
                {
                    var payload = new
                    {
                        suscripciones = listaSuscripciones,
                        asistencias = listaAsistencias,
                        reservas = listaReserva
                    };

                    string jsonPayload = JsonSerializer.Serialize(payload, new JsonSerializerOptions
                    {
                        WriteIndented = true
                    });

                    using var httpClient = new HttpClient();

                    //token
                    httpClient.DefaultRequestHeaders.Add("Authorization", "");

                    DateTime hoy = DateTime.Today;
                    int diasDesdeLunes = ((int)hoy.DayOfWeek + 6) % 7; // 0 = lunes, ..., 6 = domingo
                    DateTime lunes = hoy.AddDays(-diasDesdeLunes);

                    List<string> fechasSemana = Enumerable.Range(0, 7)
                        .Select(i => lunes.AddDays(i).ToString("yyyy-MM-dd"))
                        .ToList();

                    string fechasTexto = string.Join(", ", fechasSemana);

                    var requestBody = new
                    {
                        model = "gpt-4o",
                        messages = new[]
    {
                    new {
                        role = "system",
                        content = "Eres un modelo de regresión multivariable que analiza datos históricos de un gimnasio para predecir los horarios de menor congestión. Responde solo con JSON."
                    },
                    new {
                        role = "user",
                        content = $@"Te paso los datos históricos del gimnasio, que incluyen suscripciones activas, asistencias pasadas y reservas:

                    {jsonPayload}

                    Quiero una predicción para cada una de estas fechas: {fechasTexto}.
                    Por cada fecha, devuelve un objeto con:

                    - ""fecha"": en formato YYYY-MM-DD
                    - ""estimaciones"": una lista de bloques horarios entre 08:00 y 22:00, con 1 hora de intervalo.
                      Cada bloque debe tener:
                        - ""rango"": en formato ""08:00 - 09:00""
                        - ""asistentesPromedio"": número entero (ej: 12)

                    Devuelve un JSON con la siguiente estructura exacta (como lista, no objeto único):

                    [
                      {{
                        ""fecha"": ""2025-05-26"",
                        ""estimaciones"": [
                          {{ ""rango"": ""08:00 - 09:00"", ""asistentesPromedio"": 12 }},
                          {{ ""rango"": ""09:00 - 10:00"", ""asistentesPromedio"": 9 }}
                          ...
                        ]
                      }},
                      ...
                    ]"
                            }
                        },
                        temperature = 0.2
                    };

                    var requestJson = JsonSerializer.Serialize(requestBody);
                    var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

                    //link
                    var result = await httpClient.PostAsync("", content);
                    var resultContent = await result.Content.ReadAsStringAsync();

                    using JsonDocument doc = JsonDocument.Parse(resultContent);
                    string respuestaTexto = doc.RootElement
                        .GetProperty("choices")[0]
                        .GetProperty("message")
                        .GetProperty("content")
                        .GetString();

                    respuestaTexto = respuestaTexto
                    .Replace("```json", "")
                    .Replace("```", "")
                    .Trim();

                    if (!string.IsNullOrWhiteSpace(respuestaTexto))
                    {
                        try
                        {
                            var prediccion = JsonSerializer.Deserialize<List<ListaHorarioPredicciones>>(respuestaTexto);
                            response.data = new DataHorarioPredicciones();
                            response.data.ListaEstimaciones = prediccion;
                        }
                        catch (JsonException)
                        {

                            string jsonLimpio = JsonSerializer.Deserialize<string>(respuestaTexto);

                            var prediccion = JsonSerializer.Deserialize<List<ListaHorarioPredicciones>>(respuestaTexto);
                            response.data = new DataHorarioPredicciones();
                            response.data.ListaEstimaciones = prediccion;
                        }

                        response.codigoRes = HttpStatusCode.OK;
                        response.mensajeRes = "Predicciones obtenidos correctamente.";
                    }
                    else
                    {
                        response.codigoRes = HttpStatusCode.BadRequest;
                        response.mensajeRes = "No se obtuvieron resultados.";
                    }

                }
                else
                {
                    response.codigoRes = HttpStatusCode.BadRequest;
                    response.mensajeRes = "No se pudo obtener las Predicciones.";
                }

                return response;
            }
            catch (Exception ex)
            {
                _log.LogError("{CapacidadHorariosIA} Error: " + ex.ToString());
                return new HorarioPredicciones
                {
                    codigoRes = HttpStatusCode.InternalServerError,
                    mensajeRes = "Error interno al obtener las Predicciones. " + ex.ToString()
                };
            }
        }

    }
}
