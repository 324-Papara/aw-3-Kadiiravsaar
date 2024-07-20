namespace Papara.API.Middleware
{
	public class LoggerMiddleware
	{
		private readonly RequestDelegate _next;  // bir sonraki Middleware'i temsil edecek, geçirecek
		private readonly ILogger<LoggerMiddleware> _logger;

		public LoggerMiddleware(RequestDelegate next, ILogger<LoggerMiddleware> logger)
		{
			_next = next;
			_logger = logger;
		}

		public async Task Invoke(HttpContext context) // contex olay anında işlenen http isteği için 
		{
			// Log Request
			context.Request.EnableBuffering(); // requesti birden fazla kez okumak istersem yaparım
											   // Çünkü ASP.NET Core'da varsayılan olarak istek gövdesi bir kez okunur
											   // EnableBuffering metodunu kullandım isteğin gövdesini bellekte bir kopyasını tutmamı sağladı
			
			var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync(); // StreamReader kullanarak isteğin gövdesini okuayacak ve değişkene atayacak
																							 //  StreamReader kullandım çünkü bir akış üzerinden okuma yapacağım bu akıs üzerinden karakterleri okyacağım
																							 // ReadToEndAsync kullandım çünkü asenkron olarak tüm akışı okuyacağım ve string döneceim

			context.Request.Body.Position = 0; // işlenen isteğin gövdesinin okuma pozisyonunu 0'a yani başa döndürürüm
												// eğer isteğin body alanını başa döndürmezsem diğer bileşenler isteiği okuyamaz boş görür hep

			_logger.LogInformation($"Request: {context.Request.Method} {context.Request.Path} {requestBody}"); // loga yazıyorum

			// Capture response body
			var bodyStream = context.Response.Body; // dönecek yanıtın body içerir ve bu yanıt orijinal olan yanıttır

			using (var responseBody = new MemoryStream()) // bellekte geçiçi olarak saklamak için memorystream kullandım
														  // geçici bir MemoryStream oluşturur ve using bunu kendisi sonlandırır serbest bırakır
			{
				context.Response.Body = responseBody; // dönen yanıtı geçiçi bellek akışı(MemoryStream) ile değiştirir

				await _next(context); //  bir sonraki Middleware'i çağırdık

				// Log Response
				context.Response.Body.Seek(0, SeekOrigin.Begin);   // dönen yanıtın gövdesinin okuma pozisyonunu 0'a yani başa döndürürüm
																   // çünkü daha sonra ki akışın başından itibaren okunmasını sağlamam gerekli
																   // eekOrigin.Begin akışınn başına gitmemi sağlar seek metodu başına dönmesi gerektiğini söyler

				var responseBodyText = await new StreamReader(context.Response.Body).ReadToEndAsync(); // StreamReader kullanarak isteğin responsunu okuayacak ve değişkene atayacak
				
				context.Response.Body.Seek(0, SeekOrigin.Begin);// tekrar dönen yanıtın gövdesinin okuma pozisyonunu 0'a yani başa döndürürüm
																// neden tekrar başa döndüm çünkü orihijan akışa kopyalamadan önce gerekli doğru şekilde kopyalamam gerekli


				_logger.LogInformation($"Response: {context.Response.StatusCode} {responseBodyText}"); // loga yazarım 

				await responseBody.CopyToAsync(bodyStream); // geçici bellekten aldığım reposnse gövdesini orijinal gövdeye kopyaladım
			}
		}
	}
}