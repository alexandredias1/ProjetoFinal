using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Exceptions;
using Microsoft.VisualBasic;


class Program
{
    // Coloque seu token do bot aqui
    private static readonly string BotToken = "7435533299:AAGfOuAFkuEDqXa610k_XE3zBj2pUoRJjsw";
    private static TelegramBotClient botClient;

    static void Main(string[] args)
    {
        // Inicializa o cliente do bot com o token
        botClient = new TelegramBotClient(BotToken);

        // Configurações para o recebimento de mensagens
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = Array.Empty<UpdateType>() // Recebe todos os tipos de atualizações
        };

        // Inicia o recebimento de atualizações
        botClient.StartReceiving(HandleUpdateAsync, HandleErrorAsync, receiverOptions);

        Console.WriteLine("Bot iniciado. Pressione qualquer tecla para parar o bot...");
        Console.ReadKey();

        // Para o recebimento de mensagens
        botClient.CloseAsync().Wait();
    }

    // Método para processar as atualizações (mensagens)
    private static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        // Apenas processa mensagens de texto
        if (update.Type == UpdateType.Message && update.Message!.Text != null)
        {
            var message = update.Message;

            Console.WriteLine($"Recebi uma mensagem de {message.Chat.FirstName}: {message.Text}");

            // Verificar perguntas sobre o Rio de Janeiro
            string resposta = ProcessarPerguntaSobreRio(message.Text.ToLower());

            // Enviar a resposta para o usuário
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: resposta
            );
        }
    }

    // Método para processar perguntas relacionadas ao Rio de Janeiro
    private static string ProcessarPerguntaSobreRio(string pergunta)
    {
        if (pergunta.Contains("oi") || pergunta.Contains("ola"))
        {
            return "Olá! Como você está se sentindo hoje?";
        }
        else if (pergunta.Contains("tudo bem") || pergunta.Contains("como você está"))
        {
            return "Estou bem, obrigado por perguntar! E você, como está se sentindo?";
        }
        else if (pergunta.Contains("qual seu nome") || pergunta.Contains("quem é você"))
        {
            return "Eu sou seu assistente virtual de saúde. Como posso te ajudar hoje?";
        }
        else if (pergunta.Contains("dor") || pergunta.Contains("machucado") || pergunta.Contains("lesão"))
        {
            return "Sinto muito que você esteja com dor. Onde exatamente você está sentindo desconforto?";
        }
        else if (pergunta.Contains("medicação") || pergunta.Contains("remédio") || pergunta.Contains("remedios"))
        {
            return "Você está tomando algum medicamento no momento? Se sim, qual?";
        }
        else if (pergunta.Contains("sintoma") || pergunta.Contains("sintomas"))
        {
            return "Pode me descrever quais sintomas você está sentindo no momento?";
        }
        else if (pergunta.Contains("ajuda") || pergunta.Contains("socorro"))
        {
            return "Estou aqui para te ajudar! Me conte o que está acontecendo.";
        }
        else if (pergunta.Contains("tchau") || pergunta.Contains("adeus") || pergunta.Contains("até mais"))
        {
            return "Até mais! Se precisar de mim novamente, estarei por aqui.";
        }
        else
        {
            return "Desculpe, não entendi sua pergunta. Pode reformular?";
        }
    }
    // Método para lidar com erros
    private static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException => $"Erro na API do Telegram:\n{apiRequestException.ErrorCode}\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(ErrorMessage);
        return Task.CompletedTask;
    }
}