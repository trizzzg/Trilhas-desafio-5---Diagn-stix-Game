using System;
using System.Collections.Generic;
using UnityEngine;

public class ChestQuiz : MonoBehaviour
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

    private List<Question> chestQuestions = new List<Question>();
    private int currentQuestionIndex = 0;

    private void Start()
    {
        InitializeChestQuestions();
    }

    private void InitializeChestQuestions()
    {
        chestQuestions = new List<Question>
        {
            new Question
            {
                id = "intensidade",
                question = "1. Qual a intensidade da dor?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Leve (desconforto mínimo)", 1)},
                    {"B", ("Moderada (atrapalha atividades)", 3)},
                    {"C", ("Forte (incapacitante)", 5)},
                    {"D", ("A pior dor que já senti", 8)}
                }
            },
            new Question
            {
                id = "localizacao",
                question = "2. Onde está localizada a dor?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Centro do peito", 5)},
                    {"B", ("Lado esquerdo", 4)},
                    {"C", ("Lado direito", 3)},
                    {"D", ("Toda a região torácica", 4)},
                    {"E", ("Entre as costelas", 2)}
                }
            },
            new Question
            {
                id = "caracteristica",
                question = "3. Como descreveria a dor?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Aperto/pressão", 6)},
                    {"B", ("Pontada/agulhada", 3)},
                    {"C", ("Queimação", 4)},
                    {"D", ("Latejante", 2)}
                }
            },
            new Question
            {
                id = "duracao",
                question = "4. Há quanto tempo você está com dor?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Menos de 15 minutos", 3)},
                    {"B", ("15-30 minutos", 5)},
                    {"C", ("Mais de 30 minutos", 7)},
                    {"D", ("Vai e volta", 4)}
                }
            },
            new Question
            {
                id = "irradiacao",
                question = "5. A dor irradia para algum lugar?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Braço esquerdo", 6)},
                    {"B", ("Mandíbula/pescoço", 5)},
                    {"C", ("Costas", 4)},
                    {"D", ("Não irradia", 0)}
                }
            },
            new Question
            {
                id = "piora_respirar",
                question = "6. A dor aumenta ao respirar fundo?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, muito", 4)},
                    {"B", ("Sim, pouco", 2)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "piora_movimento",
                question = "7. A dor piora com movimento ou toque?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim", 2)},
                    {"B", ("Não", 0)}
                }
            },
            new Question
            {
                id = "palpitacoes",
                question = "8. Você sente palpitações (coração acelerado)?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, frequentes", 5)},
                    {"B", ("Sim, ocasionais", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "falta_ar",
                question = "9. Você sente falta de ar?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, em repouso", 7)},
                    {"B", ("Sim, ao esforço", 5)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "tosse",
                question = "10. Você tem tosse?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, com sangue", 8)},
                    {"B", ("Sim, seca", 3)},
                    {"C", ("Sim, com catarro", 4)},
                    {"D", ("Não", 0)}
                }
            },
            new Question
            {
                id = "febre",
                question = "11. Você tem febre?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, acima 38°C", 5)},
                    {"B", ("Sim, até 38°C", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "sudorese",
                question = "12. Você está com sudorese fria?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim", 6)},
                    {"B", ("Não", 0)}
                }
            },
            new Question
            {
                id = "nausea",
                question = "13. Você sente náuseas ou vomitou?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, vomitou", 5)},
                    {"B", ("Sim, náuseas", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "tontura",
                question = "14. Você sente tontura ou desmaio?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, quase desmaiei", 7)},
                    {"B", ("Sim, tontura", 4)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "inchaço_pernas",
                question = "15. Você tem inchaço nas pernas?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, importante", 5)},
                    {"B", ("Sim, leve", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "historia_cardiaca",
                question = "16. Você tem histórico de problemas cardíacos?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, infarto/angina", 8)},
                    {"B", ("Sim, outros", 5)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "fatores_risco",
                question = "17. Você tem fatores de risco cardíaco?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, vários (hipertensão, diabetes, colesterol)", 7)},
                    {"B", ("Sim, um ou dois", 4)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "tabagismo",
                question = "18. Você fuma?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, atualmente", 5)},
                    {"B", ("Sim, no passado", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "trauma",
                question = "19. Você sofreu trauma recente na região?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim", 4)},
                    {"B", ("Não", 0)}
                }
            },
            new Question
            {
                id = "ansiedade",
                question = "20. Você está passando por estresse ou ansiedade?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, muito", 3)},
                    {"B", ("Sim, pouco", 1)},
                    {"C", ("Não", 0)}
                }
            }
        };
    }

    public void StartChestQuiz()
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

        if (currentQuestionIndex >= chestQuestions.Count)
        {
            FinishQuiz();
            return;
        }

        Question currentQuestion = chestQuestions[currentQuestionIndex];
        questionText.text = currentQuestion.question;

        foreach (var option in currentQuestion.options)
        {
            GameObject optionButton = Instantiate(optionButtonPrefab, optionsContainer);
            TMPro.TextMeshProUGUI buttonText = optionButton.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            buttonText.text = $"{option.Key}) {option.Value.description}";

            UnityEngine.UI.Button button = optionButton.GetComponent<UnityEngine.UI.Button>();
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
        if (responses.TryGetValue("intensidade", out var respIntensidade) && 
            (respIntensidade == "Forte (incapacitante)" || respIntensidade == "A pior dor que já senti") &&
            responses.TryGetValue("caracteristica", out var respCaracteristica) && respCaracteristica == "Aperto/pressão" &&
            responses.TryGetValue("irradiacao", out var respIrradiacao) && 
            (respIrradiacao == "Braço esquerdo" || respIrradiacao == "Mandíbula/pescoço") &&
            responses.TryGetValue("sudorese", out var respSudorese) && respSudorese == "Sim")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "POSSÍVEL INFARTO AGUDO DO MIOCÁRDIO",
                recommendation = "Emergência médica! Chame SAMU 192 IMEDIATAMENTE"
            });
        }
        
        if (responses.TryGetValue("falta_ar", out var respFaltaAr) && respFaltaAr == "Sim, em repouso" &&
            responses.TryGetValue("tosse", out var respTosse) && respTosse == "Sim, com sangue")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "EMBOLIA PULMONAR",
                recommendation = "Procure atendimento URGENTE"
            });
        }

        // 2. Problemas cardíacos
        if (responses.TryGetValue("historia_cardiaca", out var respHistCardiaca) && respHistCardiaca != "Não" &&
            responses.TryGetValue("fatores_risco", out var respFatoresRisco) && respFatoresRisco != "Não" &&
            responses.TryGetValue("duracao", out var respDuracao) && 
            (respDuracao == "15-30 minutos" || respDuracao == "Mais de 30 minutos"))
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "ANGINA PECTORIS",
                recommendation = "Avaliação cardiológica urgente necessária"
            });
        }

        // 3. Problemas pulmonares
        if (responses.TryGetValue("febre", out var respFebre) && respFebre != "Não" &&
            responses.TryGetValue("tosse", out respTosse) && respTosse != "Não" &&
            responses.TryGetValue("piora_respirar", out var respPioraRespirar) && respPioraRespirar != "Não")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "PNEUMONIA OU PLEURISIA",
                recommendation = "Avaliação médica e possível radiografia"
            });
        }

        // 4. Refluxo gastroesofágico
        if (responses.TryGetValue("caracteristica", out respCaracteristica) && respCaracteristica == "Queimação" &&
            responses.TryGetValue("nausea", out var respNausea) && respNausea != "Não" &&
            responses.TryGetValue("piora_respirar", out respPioraRespirar) && respPioraRespirar == "Não")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "REFLUXO GASTROESOFÁGICO",
                recommendation = "Antiácidos podem ajudar - evite deitar após comer"
            });
        }

        // 5. Costocondrite
        if (responses.TryGetValue("piora_movimento", out var respPioraMovimento) && respPioraMovimento == "Sim" &&
            responses.TryGetValue("piora_respirar", out respPioraRespirar) && respPioraRespirar != "Não" &&
            responses.TryGetValue("localizacao", out var respLocalizacao) && respLocalizacao == "Entre as costelas")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "COSTOCONDRITE",
                recommendation = "Inflamação das cartilagens costais - anti-inflamatórios podem ajudar"
            });
        }

        // 6. Ansiedade
        if (responses.TryGetValue("ansiedade", out var respAnsiedade) && respAnsiedade != "Não" &&
            responses.TryGetValue("palpitacoes", out var respPalpitacoes) && respPalpitacoes != "Não" &&
            responses.TryGetValue("caracteristica", out respCaracteristica) && respCaracteristica == "Pontada/agulhada")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "CRISE DE ANSIEDADE",
                recommendation = "Técnicas de respiração podem ajudar - avalie com psicólogo"
            });
        }

        // 7. Pneumotórax
        if (responses.TryGetValue("falta_ar", out respFaltaAr) && respFaltaAr != "Não" &&
            responses.TryGetValue("intensidade", out respIntensidade) && respIntensidade == "Forte (incapacitante)" &&
            responses.TryGetValue("piora_respirar", out respPioraRespirar) && respPioraRespirar == "Sim, muito")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "POSSÍVEL PNEUMOTÓRAX",
                recommendation = "Avaliação médica urgente necessária"
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
            resultText.text = "Nenhuma condição específica identificada";
        }

        // Classificação por pontuação
        riskLevelText.text = "NÍVEL DE RISCO GERAL:\n";
        if (totalScore >= 50)
        {
            riskLevelText.text += "RISCO MUITO ELEVADO - Procure ajuda profissional IMEDIATA";
        }
        else if (totalScore >= 30)
        {
            riskLevelText.text += "RISCO MODERADO/ALTO - Agende avaliação médica em até 48h";
        }
        else if (totalScore >= 15)
        {
            riskLevelText.text += "RISCO LEVE - Monitore sintomas e consulte se persistirem";
        }
        else
        {
            riskLevelText.text += "BAIXO RISCO - Mantenha hábitos saudáveis";
        }

        scoreText.text = $"Pontuação total: {totalScore}/120";

    }
}