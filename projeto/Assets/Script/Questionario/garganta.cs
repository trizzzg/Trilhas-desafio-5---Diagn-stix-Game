using System;
using System.Collections.Generic;
using UnityEngine;

public class ThroatQuiz : MonoBehaviour
{
    [Serializable]
    public class Question
    {
        public string id;
        public string question;
        public Dictionary<string, (string description, int points)> options;
    }

    [Serializable]
    public class Diagnosis
    {
        public string condition;
        public string recommendation;
    }

    private Dictionary<string, string> responses = new Dictionary<string, string>();
    private int totalScore = 0;
    private List<Diagnosis> diagnoses = new List<Diagnosis>();

    // UI References (assign in Inspector)
    public GameObject quizPanel;
    public TMPro.TextMeshProUGUI questionText;
    public GameObject optionButtonPrefab;
    public Transform optionsContainer;
    public TMPro.TextMeshProUGUI resultText;
    public GameObject diagnosisPrefab;
    public Transform diagnosesContainer;
    public TMPro.TextMeshProUGUI scoreText;
    public TMPro.TextMeshProUGUI riskLevelText;
    public GameObject resultPanel;
    public GameObject options;


    private List<Question> throatQuestions = new List<Question>();
    private int currentQuestionIndex = 0;

    private void Start()
    {
        InitializeThroatQuestions();

    }

    private void InitializeThroatQuestions()
    {
        throatQuestions = new List<Question>
        {
            new Question
            {
                id = "intensidade",
                question = "1. Qual a intensidade da dor?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Leve (desconforto mínimo)", 1)},
                    {"B", ("Moderada (atrapalha alimentação)", 3)},
                    {"C", ("Forte (dificuldade para engolir saliva)", 5)},
                    {"D", ("Insuportável", 7)}
                }
            },
            new Question
            {
                id = "dificuldade_engolir",
                question = "2. Você sente dificuldade para engolir?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, até líquidos", 6)},
                    {"B", ("Sim, apenas sólidos", 4)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "duracao",
                question = "3. Há quanto tempo sente os sintomas?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Menos de 24 horas", 1)},
                    {"B", ("1-3 dias", 3)},
                    {"C", ("4-7 dias", 5)},
                    {"D", ("Mais de 1 semana", 7)}
                }
            },
            new Question
            {
                id = "febre",
                question = "4. Você tem febre? Se sim, qual a temperatura aproximada?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Não tenho febre", 0)},
                    {"B", ("Sim, até 38°C (febre baixa)", 3)},
                    {"C", ("Sim, acima de 38°C (febre alta)", 5)},
                    {"D", ("Não medi, mas sinto calafrios", 2)}
                }
            },
            new Question
            {
                id = "placas_pus",
                question = "5. Há presença de placas brancas ou pus na garganta?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, muitas placas", 6)},
                    {"B", ("Sim, poucas placas", 4)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "rouquidao",
                question = "6. Você tem rouquidão ou alteração na voz?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, voz quase inaudível", 5)},
                    {"B", ("Sim, voz rouca", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "ganglios",
                question = "7. Você sente gânglios (caroços) inchados no pescoço?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, dolorosos", 5)},
                    {"B", ("Sim, indolores", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "tosse",
                question = "8. Você está tossindo?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, tosse seca intensa", 4)},
                    {"B", ("Sim, tosse com catarro", 3)},
                    {"C", ("Sim, tosse rouca", 5)},
                    {"D", ("Não", 0)}
                }
            },
            new Question
            {
                id = "dores_corpo",
                question = "9. Você sente dores no corpo ou cansaço excessivo?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, intensos", 4)},
                    {"B", ("Sim, moderados", 2)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "congestao",
                question = "10. Você tem congestão nasal ou coriza?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, intensa", 2)},
                    {"B", ("Sim, leve", 1)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "olfato_paladar",
                question = "11. Você perdeu o olfato ou paladar recentemente?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, completamente", 6)},
                    {"B", ("Sim, parcialmente", 4)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "contato_covid",
                question = "12. Você teve contato com alguém com COVID-19 nos últimos 14 dias?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, contato próximo", 8)},
                    {"B", ("Sim, contato casual", 4)},
                    {"C", ("Não", 0)},
                    {"D", ("Não sei", 2)}
                }
            },
            new Question
            {
                id = "erupcao_cutanea",
                question = "13. Você apresenta erupção cutânea ou manchas na pele?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim", 5)},
                    {"B", ("Não", 0)}
                }
            },
            new Question
            {
                id = "dificuldade_respirar",
                question = "14. Você sente dificuldade para respirar?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, importante", 10)},
                    {"B", ("Sim, leve", 5)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "historia_amigdalite",
                question = "15. Você tem histórico de amigdalites frequentes?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, mais de 3 por ano", 4)},
                    {"B", ("Sim, ocasionais", 2)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "alergias",
                question = "16. Você tem alergias conhecidas (poeira, pólen, etc.)?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, graves", 3)},
                    {"B", ("Sim, leves", 1)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "fumante",
                question = "17. Você fuma ou convive com fumantes?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, fumo ativamente", 5)},
                    {"B", ("Sim, convivo com fumantes", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "antibiotico_recente",
                question = "18. Usou antibióticos nos últimos 3 meses?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, múltiplas vezes", 4)},
                    {"B", ("Sim, uma vez", 2)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "imunossupressao",
                question = "19. Você tem condição que afete sua imunidade?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim (HIV, quimioterapia, etc.)", 8)},
                    {"B", ("Não sei", 1)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "viagem_recente",
                question = "20. Viajou para áreas endêmicas recentemente?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim", 5)},
                    {"B", ("Não", 0)}
                }
            }
        };
    }

    public void StartThroatQuiz()
    {
        options.SetActive(false);
        responses.Clear();
        totalScore = 0;
        diagnoses.Clear();
        currentQuestionIndex = 0;
        quizPanel.SetActive(true);
        ShowCurrentQuestion();
    }

    private void ShowCurrentQuestion()
    {
        foreach (Transform child in optionsContainer)
            Destroy(child.gameObject);

        if (currentQuestionIndex >= throatQuestions.Count)
        {
            FinishQuiz();
            return;
        }

        Question currentQuestion = throatQuestions[currentQuestionIndex];
        questionText.text = currentQuestion.question;

        foreach (var option in currentQuestion.options)
        {
            GameObject optionButton = Instantiate(optionButtonPrefab, optionsContainer);
            TMPro.TextMeshProUGUI buttonText = optionButton.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            buttonText.text = $"{option.Key}) {option.Value.description}";

            UnityEngine.UI.Button button = optionButton.GetComponent<UnityEngine.UI.Button>();
            // Need to capture option.Key and currentQuestion by local variables to avoid closure issues
            string keyCopy = option.Key;
            Question questionCopy = currentQuestion;
            button.onClick.AddListener(() => OnOptionSelected(keyCopy, questionCopy));
        }
    }

    private void OnOptionSelected(string optionKey, Question question)
    {
        if (question.options.TryGetValue(optionKey, out var option))
        {
            responses[question.id] = option.description;
            totalScore += option.points;
            currentQuestionIndex++;
            ShowCurrentQuestion();
        }
    }

    private void FinishQuiz()
    {
    quizPanel.SetActive(false);      
    resultPanel.SetActive(true);     
    EvaluateDiagnoses();
    DisplayResults();
    }

    private void EvaluateDiagnoses()
    {
        diagnoses.Clear();

        // 1. Emergências (prioridade máxima)
        if (responses.TryGetValue("dificuldade_respirar", out var respRespirar) && respRespirar == "Sim, importante")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "DIFICULDADE RESPIRATÓRIA GRAVE",
                recommendation = "Procure atendimento médico IMEDIATO"
            });
        }

        if (responses.TryGetValue("dificuldade_engolir", out var respEngolir) && respEngolir == "Sim, até líquidos" &&
            responses.TryGetValue("intensidade", out var respIntensidade) &&
            (respIntensidade == "Forte (dificuldade para engolir saliva)" || respIntensidade == "Insuportável"))
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "OBSTRUÇÃO POTENCIAL DA GARGANTA",
                recommendation = "Emergência médica - risco de asfixia"
            });
        }

        // 2. COVID-19
        bool suspeitaCovid = false;
        if (responses.TryGetValue("olfato_paladar", out var respOlfato) && respOlfato != "Não")
            suspeitaCovid = true;

        if (responses.TryGetValue("contato_covid", out var respContatoCovid) &&
            (respContatoCovid == "Sim, contato próximo" || respContatoCovid == "Sim, contato casual"))
            suspeitaCovid = true;

        if (responses.TryGetValue("febre", out var respFebre) && respFebre != "Não tenho febre" &&
            responses.TryGetValue("tosse", out var respTosse) && respTosse != "Não" &&
            responses.TryGetValue("dores_corpo", out var respDores) && respDores != "Não")
            suspeitaCovid = true;

        if (suspeitaCovid)
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "SUSPEITA DE COVID-19",
                recommendation = "Isole-se e faça teste PCR. Monitore saturação de O2"
            });
        }

        // 3. Amigdalite bacteriana
        if (responses.TryGetValue("placas_pus", out var respPlacas) && respPlacas != "Não" &&
            responses.TryGetValue("febre", out respFebre) && respFebre != "Não tenho febre" &&
            responses.TryGetValue("ganglios", out var respGanglios) && respGanglios != "Não")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "AMIGDALITE BACTERIANA",
                recommendation = "Pode requerer antibióticos - consulte médico"
            });
        }

        // 4. Mononucleose
        if (responses.TryGetValue("febre", out respFebre) && respFebre == "Sim, acima de 38°C (febre alta)" &&
            responses.TryGetValue("ganglios", out respGanglios) && respGanglios == "Sim, dolorosos" &&
            responses.TryGetValue("dores_corpo", out respDores) && respDores != "Não" &&
            responses.TryGetValue("duracao", out var respDuracao) &&
            (respDuracao == "4-7 dias" || respDuracao == "Mais de 1 semana"))
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "MONONUCLEOSE INFECCIOSA",
                recommendation = "Repouso e hidratação são essenciais"
            });
        }

        // 5. Faringite estreptocócica
        if (responses.TryGetValue("placas_pus", out respPlacas) && respPlacas != "Não" &&
            responses.TryGetValue("febre", out respFebre) && respFebre != "Não tenho febre" &&
            responses.TryGetValue("historia_amigdalite", out var respHistAmigdalite) && respHistAmigdalite != "Não")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "FARINGITE ESTREPTOCÓCICA",
                recommendation = "Teste rápido pode confirmar - tratamento com antibióticos"
            });
        }

        // 6. Laringite
        if (responses.TryGetValue("rouquidao", out var respRouquidao) && respRouquidao != "Não" &&
            responses.TryGetValue("tosse", out respTosse) && (respTosse == "Sim, tosse rouca" || respTosse == "Sim, tosse seca intensa") &&
            responses.TryGetValue("febre", out respFebre) && respFebre == "Não tenho febre")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "LARINGITE",
                recommendation = "Repouso vocal e hidratação"
            });
        }

        // 7. Alergias
        if (responses.TryGetValue("alergias", out var respAlergias) && respAlergias != "Não" &&
            responses.TryGetValue("congestao", out var respCongestao) && respCongestao != "Não" &&
            responses.TryGetValue("febre", out respFebre) && respFebre == "Não tenho febre")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "REAÇÃO ALÉRGICA",
                recommendation = "Anti-histamínicos podem ajudar - evite alérgenos"
            });
        }

        // 8. Difteria (em áreas endêmicas)
        if (responses.TryGetValue("viagem_recente", out var respViagem) && respViagem == "Sim" &&
            responses.TryGetValue("placas_pus", out respPlacas) && respPlacas == "Sim, muitas placas" &&
            responses.TryGetValue("dificuldade_respirar", out var respDificResp) && respDificResp != "Não")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "SUSPEITA DE DIFTERIA",
                recommendation = "Emergência médica - doença grave e contagiosa"
            });
        }
    }

    private void DisplayResults()
    {
        // Limpa resultados anteriores
        foreach (Transform child in diagnosesContainer)
        {
            Destroy(child.gameObject);
        }

        if (diagnoses.Count > 0)
        {
            resultText.text = "🔍 DIAGNÓSTICOS IDENTIFICADOS:";
            for (int i = 0; i < diagnoses.Count; i++)
            {
                GameObject diagnosisObj = Instantiate(diagnosisPrefab, diagnosesContainer);
                TMPro.TextMeshProUGUI diagnosisText = diagnosisObj.GetComponent<TMPro.TextMeshProUGUI>();
                diagnosisText.text = $"{i + 1}. {diagnoses[i].condition}\n→ {diagnoses[i].recommendation}";
            }
        }
        else
        {
            resultText.text = "🟢 Nenhuma condição específica identificada";
        }

        // Classificação por pontuação
        riskLevelText.text = "NÍVEL DE RISCO GERAL:\n";
        if (totalScore >= 50)
        {
            riskLevelText.text += "RISCO MUITO ELEVADO - Procure ajuda profissional IMEDIATA";
        }
        else if (totalScore >= 30)
        {
            riskLevelText.text += "RISCO MODERADO/ALTO - Agende avaliação médica em até 24h";
        }
        else if (totalScore >= 15)
        {
            riskLevelText.text += "RISCO LEVE - Monitore sintomas e consulte se persistirem";
        }
        else
        {
            riskLevelText.text += "BAIXO RISCO - Cuide da saúde e mantenha hábitos adequados";
        }

        scoreText.text = $"Pontuação total: {totalScore}/100+";

        // Opcional: resumo das respostas pode ser adicionado aqui
    }
}
