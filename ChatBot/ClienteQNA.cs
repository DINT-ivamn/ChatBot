using Microsoft.Azure.CognitiveServices.Knowledge.QnAMaker;
using Microsoft.Azure.CognitiveServices.Knowledge.QnAMaker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ChatBot
{
    public class ClienteQnA
    {
        private QnAMakerRuntimeClient Cliente { get; }
        private string Id { get;}

        public ClienteQnA()
        {
            string EndPoint = Properties.Settings.Default.Endpoint;
            string Key = Properties.Settings.Default.Key;
            Id = Properties.Settings.Default.Id;
            Cliente = new QnAMakerRuntimeClient(new EndpointKeyServiceClientCredentials(Key)) { RuntimeEndpoint = EndPoint };
        }

        public async Task<string> PreguntarAsync(string pregunta)
        {
            QnASearchResultList response = await Cliente.Runtime.GenerateAnswerAsync(Id, new QueryDTO { Question = pregunta });
            return response.Answers[0].Answer;
        }
    }
}
