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

            // Verificar perguntas sobre o paciente
            string resposta = ProcessarPerguntaSobrePaciente(message.Text.ToLower());

            // Enviar a resposta para o usuário
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: resposta
            );
        }
    }

    // Método para processar perguntas relacionadas ao paciente
    private static string ProcessarPerguntaSobrePaciente(string pergunta)
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
        else if (pergunta.Contains("exercício") || pergunta.Contains("atividade física"))
        {
            return "Você pratica algum exercício regularmente? Exercícios podem ajudar muito com sua saúde.";
        }
        else if (pergunta.Contains("alimentação") || pergunta.Contains("comida") || pergunta.Contains("nutrição"))
        {
            return "Uma alimentação saudável é essencial para o bem-estar. Você tem alguma dúvida sobre sua dieta?";
        }
        else if (pergunta.Contains("sono") || pergunta.Contains("dormir"))
        {
            return "O sono é fundamental para a saúde. Quantas horas de sono você costuma ter por noite?";
        }
        else if (pergunta.Contains("estresse") || pergunta.Contains("ansiedade"))
        {
            return "Entendo que o estresse pode ser difícil. Você gostaria de algumas dicas para relaxar?";
        }
        else if (pergunta.Contains("hidratação") || pergunta.Contains("água"))
        {
            return "Beber água suficiente é muito importante. Quantos copos de água você bebe por dia?";
        }
        else if (pergunta.Contains("pressão") || pergunta.Contains("pressão alta") || pergunta.Contains("hipertensão"))
        {
            return "Você está monitorando sua pressão arterial? É importante manter ela controlada.";
        }
        else if (pergunta.Contains("diabetes") || pergunta.Contains("açúcar no sangue") || pergunta.Contains("glicose"))
        {
            return "Como está sua glicemia? Monitorar o nível de açúcar no sangue é essencial para quem tem diabetes.";
        }
        else if (pergunta.Contains("covid") || pergunta.Contains("corona") || pergunta.Contains("vírus"))
        {
            return "Você está apresentando algum sintoma de COVID-19, como febre, tosse ou falta de ar?";
        }
        else if (pergunta.Contains("vacina") || pergunta.Contains("vacinação"))
        {
            return "As vacinas são importantes para prevenir doenças. Você está com suas vacinas em dia?";
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