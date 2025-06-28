using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SkinQuiz : MonoBehaviour
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

    [Serializable]
    public class UserInfo
    {
        public string userId;
        public string username;
    }

    [Serializable]
    public class QuizResult
    {
        public UserInfo user;
        public Dictionary<string, string> responses;
        public int totalScore;
        public List<Diagnosis> diagnoses;
        public string riskLevel;
        public string timestamp;
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

    private List<Question> skinQuestions = new List<Question>();
    private int currentQuestionIndex = 0;

    private void Start()
    {
        InitializeSkinQuestions();
    }

    private void InitializeSkinQuestions()
    {
        skinQuestions = new List<Question>
        {
            new Question
            {
                id = "intensidade",
                question = "1. Qual a intensidade do desconforto?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Leve (irritação passageira)", 1)},
                    {"B", ("Moderada (atrapalha atividades)", 3)},
                    {"C", ("Forte (dor/coceira intensa)", 5)},
                    {"D", ("Extrema (insuportável)", 8)}
                }
            },
            new Question
            {
                id = "tipo_sintoma",
                question = "2. Qual o principal sintoma?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Vermelhidão", 3)},
                    {"B", ("Coceira intensa", 4)},
                    {"C", ("Erupções/bolhas", 5)},
                    {"D", ("Descamação", 3)},
                    {"E", ("Dor/ardência", 4)},
                    {"F", ("Manchas escuras/claras", 3)}
                }
            },
            new Question
            {
                id = "duracao",
                question = "3. Há quanto tempo você está com os sintomas?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Menos de 6 horas", 1)},
                    {"B", ("6-24 horas", 3)},
                    {"C", ("1-3 dias", 5)},
                    {"D", ("Mais de 3 dias", 7)}
                }
            },
            new Question
            {
                id = "localizacao",
                question = "4. Onde está localizado o problema?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Rosto", 3)},
                    {"B", ("Mãos/pés", 3)},
                    {"C", ("Tronco", 2)},
                    {"D", ("Dobras (cotovelos/joelhos)", 4)},
                    {"E", ("Generalizado", 5)}
                }
            },
            new Question
            {
                id = "piora_calor",
                question = "5. Os sintomas pioram com calor ou suor?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, muito", 4)},
                    {"B", ("Sim, pouco", 2)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "secrecao",
                question = "6. Há secreção/pus nas lesões?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, purulenta (amarela/esverdeada)", 6)},
                    {"B", ("Sim, aquosa/transparente", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "produto_novo",
                question = "7. Usou produtos novos recentemente?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, cosmético/creme", 5)},
                    {"B", ("Sim, medicamento tópico", 4)},
                    {"C", ("Sim, medicamento oral", 6)},
                    {"D", ("Não", 0)}
                }
            },
            new Question
            {
                id = "alergia_historico",
                question = "8. Tem histórico de alergias ou dermatite?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, múltiplas alergias", 5)},
                    {"B", ("Sim, alergia específica", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "espalhamento",
                question = "9. As lesões estão se espalhando?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, rapidamente", 7)},
                    {"B", ("Sim, lentamente", 4)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "febre",
                question = "10. Você tem febre ou mal-estar?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, febre alta (>38°C)", 7)},
                    {"B", ("Sim, febre baixa", 4)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "contato_doente",
                question = "11. Teve contato com alguém com erupção cutânea?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim", 5)},
                    {"B", ("Não", 0)}
                }
            },
            new Question
            {
                id = "bolhas",
                question = "12. Há formação de bolhas?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, grandes", 6)},
                    {"B", ("Sim, pequenas", 4)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "sol",
                question = "13. Piora com exposição solar?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim", 4)},
                    {"B", ("Não", 0)}
                }
            },
            new Question
            {
                id = "coceira_noturna",
                question = "14. A coceira piora à noite?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim", 5)},
                    {"B", ("Não", 0)}
                }
            },
            new Question
            {
                id = "linhas",
                question = "15. Notou linhas finas na pele?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim", 6)},
                    {"B", ("Não", 0)}
                }
            },
            new Question
            {
                id = "psoriase_historico",
                question = "16. Há histórico de psoríase na família?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim", 4)},
                    {"B", ("Não", 0)}
                }
            },
            new Question
            {
                id = "imunossupressao",
                question = "17. Tem condição que afete sua imunidade?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim (HIV, quimioterapia, etc.)", 7)},
                    {"B", ("Não", 0)}
                }
            },
            new Question
            {
                id = "animais",
                question = "18. Tem contato com animais?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, novos", 4)},
                    {"B", ("Sim, habituais", 2)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "viagem",
                question = "19. Viajou recentemente?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, para áreas endêmicas", 5)},
                    {"B", ("Sim, para outras regiões", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "estresse",
                question = "20. Está passando por período de estresse?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, intenso", 3)},
                    {"B", ("Sim, moderado", 2)},
                    {"C", ("Não", 0)}
                }
            }
        };
    }

    public void StartSkinQuiz()
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

        if (currentQuestionIndex >= skinQuestions.Count)
        {
            FinishQuiz();
            return;
        }

        Question currentQuestion = skinQuestions[currentQuestionIndex];
        questionText.text = currentQuestion.question;

        foreach (var option in currentQuestion.options)
        {
            GameObject optionButton = Instantiate(optionButtonPrefab, optionsContainer);
            TMPro.TextMeshProUGUI buttonText = optionButton.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            buttonText.text = $"{option.Key}) {option.Value.description}";

            UnityEngine.UI.Button button = optionButton.GetComponent<UnityEngine.UI.Button>();
            string keyCopy = option.Key;
            button.onClick.AddListener(() => OnOptionSelected(keyCopy, currentQuestion));
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
        SaveResultsToJson();
    }

    private void EvaluateDiagnoses()
    {
        diagnoses.Clear();

        // 1. Emergency conditions (priority)
        if (responses.TryGetValue("febre", out var respFebre) && respFebre == "Sim, febre alta (>38°C)" &&
            responses.TryGetValue("tipo_sintoma", out var respTipo) && respTipo == "Erupções/bolhas")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "INFECÇÃO GRAVE (CELULITE/ERISIPELA)",
                recommendation = "Emergência médica! Risco de sepse - procure atendimento IMEDIATO"
            });
        }
        
        if (responses.TryGetValue("espalhamento", out var respEspalhamento) && respEspalhamento == "Sim, rapidamente" &&
            responses.TryGetValue("bolhas", out var respBolhas) && respBolhas == "Sim, grandes")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "DERMATITE ESFOLIATIVA OU PEMFIGO",
                recommendation = "Condição grave - internação pode ser necessária"
            });
        }

        if (responses.TryGetValue("linhas", out var respLinhas) && respLinhas == "Sim" &&
            responses.TryGetValue("coceira_noturna", out var respCoceira) && respCoceira == "Sim")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "ESCABIOSE (SARNA)",
                recommendation = "Contagioso - requer tratamento específico para você e contatos"
            });
        }

        // 2. Infections
        if (responses.TryGetValue("secrecao", out var respSecrecao) && respSecrecao == "Sim, purulenta (amarela/esverdeada)" &&
            responses.TryGetValue("localizacao", out var respLocalizacao) && 
            (respLocalizacao == "Rosto" || respLocalizacao == "Mãos/pés"))
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "IMPETIGO OU FOLICULITE",
                recommendation = "Pode requerer antibiótico tópico ou oral"
            });
        }

        if (responses.TryGetValue("contato_doente", out var respContato) && respContato == "Sim" &&
            responses.TryGetValue("tipo_sintoma", out respTipo) && respTipo == "Erupções/bolhas")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "DOENÇA VIRAL (CATAPORA/ZIKA)",
                recommendation = "Isolamento pode ser necessário"
            });
        }

        if (responses.TryGetValue("viagem", out var respViagem) && respViagem == "Sim, para áreas endêmicas" &&
            responses.TryGetValue("febre", out respFebre) && respFebre != "Não")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "DOENÇA TROPICAL (DENGUE/CHIKUNGUNYA)",
                recommendation = "Avaliação médica e exames específicos"
            });
        }

        // 3. Dermatitis
        if (responses.TryGetValue("produto_novo", out var respProduto) && respProduto != "Não" &&
            responses.TryGetValue("alergia_historico", out var respAlergia) && respAlergia != "Não")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "DERMATITE DE CONTATO",
                recommendation = "Identifique e remova o agente causador"
            });
        }

        if (responses.TryGetValue("localizacao", out respLocalizacao) && respLocalizacao == "Dobras (cotovelos/joelhos)" &&
            responses.TryGetValue("psoriase_historico", out var respPsoriase) && respPsoriase == "Sim")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "PSORÍASE OU DERMATITE ATÓPICA",
                recommendation = "Hidratantes e corticoides tópicos podem ajudar"
            });
        }

        // 4. Other conditions
        if (responses.TryGetValue("piora_calor", out var respCalor) && respCalor == "Sim, muito" &&
            responses.TryGetValue("tipo_sintoma", out respTipo) && respTipo == "Coceira intensa")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "BROTOEJA (MILIÁRIA)",
                recommendation = "Mantenha a pele fresca e seca"
            });
        }

        if (responses.TryGetValue("sol", out var respSol) && respSol == "Sim" &&
            responses.TryGetValue("tipo_sintoma", out respTipo) && respTipo == "Manchas escuras/claras")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "FOTODERMATITE OU MELASMA",
                recommendation = "Use protetor solar FPS 50+"
            });
        }
    }

    private void DisplayResults()
    {
        foreach (Transform child in diagnosesContainer)
        {
            Destroy(child.gameObject);
        }

        if (diagnoses.Count > 0)
        {
            resultText.text = "DIAGNÓSTICOS IDENTIFICADOS:";
            int maxDiagnosesToShow = Mathf.Min(diagnoses.Count, 2);

            for (int i = 0; i < maxDiagnosesToShow; i++)
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

        riskLevelText.text = "NÍVEL DE RISCO GERAL:\n" + GetRiskLevelText();
        scoreText.text = $"Pontuação total: {totalScore}/120";
    }

    private string GetRiskLevelText()
    {
        if (totalScore >= 50) return "🚨 RISCO MUITO ELEVADO - Procure ajuda dermatológica IMEDIATA";
        if (totalScore >= 30) return "⚠️ RISCO MODERADO/ALTO - Agende avaliação em até 48h";
        if (totalScore >= 15) return "🔍 RISCO LEVE - Monitore sintomas e consulte se persistirem";
        return "✅ BAIXO RISCO - Mantenha cuidados básicos com a pele";
    }

    private string GetRiskLevel()
    {
        if (totalScore >= 50) return "MUITO ELEVADO";
        if (totalScore >= 30) return "ALTO";
        if (totalScore >= 15) return "MODERADO";
        return "BAIXO";
    }

    private void SaveResultsToJson()
    {
        QuizResult result = new QuizResult
        {
            user = new UserInfo
            {
                userId = "usr123",  // Substituir por UsuarioLogado.userId quando tiver o login
                username = "João Silva" // Substituir por UsuarioLogado.username quando tiver o login
            },
            responses = responses,
            totalScore = totalScore,
            diagnoses = diagnoses,
            riskLevel = GetRiskLevel(),
            timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };

        string json = JsonUtility.ToJson(result, true);
        string path = Path.Combine(Application.persistentDataPath, "diagnostico_pele.json");
        File.WriteAllText(path, json);
        Debug.Log("Resultados salvos em: " + path);
    }
}