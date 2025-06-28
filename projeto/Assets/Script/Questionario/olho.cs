using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EyeQuiz : MonoBehaviour
{
    [System.Serializable]
    public class Question
    {
        public string id;
        public string question;
        public Dictionary<string, (string description, int points)> options;
    }

    [System.Serializable]
    public class Diagnosis
    {
        public string condition;
        public string recommendation;
    }

    [System.Serializable]
    public class UserInfo
    {
        public string userId;
        public string username;
    }

    [System.Serializable]
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

    // UI References (to be assigned in Unity Inspector)
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

    private List<Question> eyeQuestions = new List<Question>();
    private int currentQuestionIndex = 0;

    private void Start()
    {
        InitializeEyeQuestions();
    }

    private void InitializeEyeQuestions()
    {
        eyeQuestions = new List<Question>
        {
            new Question
            {
                id = "intensidade",
                question = "1. Qual a intensidade do desconforto?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Leve (irritação passageira)", 1)},
                    {"B", ("Moderada (atrapalha atividades)", 3)},
                    {"C", ("Forte (dor incapacitante)", 5)},
                    {"D", ("Extrema (visão comprometida)", 8)}
                }
            },
            new Question
            {
                id = "tipo_sintoma",
                question = "2. Qual o principal sintoma?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Vermelhidão", 3)},
                    {"B", ("Dor/ardência intensa", 5)},
                    {"C", ("Coceira", 2)},
                    {"D", ("Visão embaçada", 6)},
                    {"E", ("Sensibilidade à luz", 4)},
                    {"F", ("Corpos flutuantes/moscas volantes", 4)}
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
                id = "piora_telas",
                question = "4. Os sintomas pioram ao usar telas ou ler?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, significativamente", 4)},
                    {"B", ("Sim, levemente", 2)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "secrecao",
                question = "5. Você notou secreção ocular?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, purulenta (amarela/esverdeada)", 6)},
                    {"B", ("Sim, aquosa/transparente", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "visao_alterada",
                question = "6. Como está sua acuidade visual?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Piorou abruptamente", 8)},
                    {"B", ("Piorou gradualmente", 5)},
                    {"C", ("Sem alterações", 0)}
                }
            },
            new Question
            {
                id = "dor_profunda",
                question = "7. Você sente dor profunda no olho ou ao redor?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, intensa", 7)},
                    {"B", ("Sim, moderada", 4)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "halos_luminosos",
                question = "8. Você vê halos ao redor de luzes?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, frequentemente", 6)},
                    {"B", ("Sim, ocasionalmente", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "fotofobia",
                question = "9. A sensibilidade à luz é incapacitante?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, não consigo abrir os olhos", 7)},
                    {"B", ("Sim, desconfortável", 4)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "olho_seco",
                question = "10. Você sente os olhos secos ou arenosos?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, constantemente", 5)},
                    {"B", ("Sim, às vezes", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "lentes_contato",
                question = "11. Você usa lentes de contato?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, com uso prolongado", 6)},
                    {"B", ("Sim, com uso adequado", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "historia_glaucoma",
                question = "12. Há histórico de glaucoma na família?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, em parentes próximos", 5)},
                    {"B", ("Não sei", 2)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "diabetes",
                question = "13. Você tem diabetes?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim", 4)},
                    {"B", ("Não", 0)}
                }
            },
            new Question
            {
                id = "pressao_alta",
                question = "14. Você tem hipertensão arterial?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim", 4)},
                    {"B", ("Não", 0)}
                }
            },
            new Question
            {
                id = "inchaço_palpebras",
                question = "15. Há inchaço nas pálpebras?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, com vermelhidão", 5)},
                    {"B", ("Sim, sem vermelhidão", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "trauma",
                question = "16. Sofreu trauma ocular recente?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, com objeto pontiagudo", 10)},
                    {"B", ("Sim, com impacto", 6)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "quimicos",
                question = "17. Teve contato com produtos químicos?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim", 8)},
                    {"B", ("Não", 0)}
                }
            },
            new Question
            {
                id = "cefaleia",
                question = "18. Você está com dor de cabeça intensa?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, com náuseas", 6)},
                    {"B", ("Sim, sem náuseas", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "febre",
                question = "19. Você tem febre?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, acima de 38°C", 5)},
                    {"B", ("Sim, até 38°C", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "uso_colirios",
                question = "20. Usou colírios ou medicamentos oculares?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, sem prescrição", 4)},
                    {"B", ("Sim, com prescrição", 2)},
                    {"C", ("Não", 0)}
                }
            }
        };
    }

    public void StartEyeQuiz()
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
        // Clear previous options
        foreach (Transform child in optionsContainer)
        {
            Destroy(child.gameObject);
        }

        if (currentQuestionIndex >= eyeQuestions.Count)
        {
            FinishQuiz();
            return;
        }

        Question currentQuestion = eyeQuestions[currentQuestionIndex];
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
        SaveResultsToJson();
    }

    private void EvaluateDiagnoses()
    {
        diagnoses.Clear();

        // 1. Emergency conditions (3 diagnoses)
        if (responses.TryGetValue("visao_alterada", out var respVisao) && respVisao == "Piorou abruptamente" &&
            responses.TryGetValue("dor_profunda", out var respDor) && respDor != "Não")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "GLAUCOMA AGUDO OU UVEÍTE",
                recommendation = "EMERGÊNCIA OFTALMOLÓGICA! Risco de perda visual permanente - procure atendimento IMEDIATO"
            });
        }
        
        if (responses.TryGetValue("trauma", out var respTrauma) && respTrauma != "Não" ||
            responses.TryGetValue("quimicos", out var respQuimicos) && respQuimicos == "Sim")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "TRAUMA OU QUEIMADURA OCULAR",
                recommendation = "Lave com água corrente por 15 minutos e procure oftalmologista URGENTEMENTE"
            });
        }

        if (responses.TryGetValue("visao_alterada", out respVisao) && respVisao == "Piorou abruptamente" &&
            responses.TryGetValue("halos_luminosos", out var respHalos) && respHalos != "Não" &&
            responses.TryGetValue("cefaleia", out var respCefaleia) && respCefaleia != "Não")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "CRISE DE GLAUCOMA AGUDO",
                recommendation = "Redução da pressão intraocular urgente necessária"
            });
        }

        // 2. Infections/inflammations (3 diagnoses)
        if (responses.TryGetValue("secrecao", out var respSecrecao) && respSecrecao == "Sim, purulenta (amarela/esverdeada)" &&
            responses.TryGetValue("inchaço_palpebras", out var respInchaco) && respInchaco != "Não")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "CONJUNTIVITE BACTERIANA",
                recommendation = "Requer colírios antibióticos prescritos - evite automedicação"
            });
        }

        if (responses.TryGetValue("tipo_sintoma", out var respSintoma) && respSintoma == "Coceira" &&
            responses.TryGetValue("secrecao", out respSecrecao) && respSecrecao == "Sim, aquosa/transparente")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "CONJUNTIVITE ALÉRGICA",
                recommendation = "Anti-histamínicos oculares e compressas frias podem ajudar"
            });
        }

        if (responses.TryGetValue("inchaço_palpebras", out respInchaco) && respInchaco == "Sim, com vermelhidão" &&
            responses.TryGetValue("dor_profunda", out respDor) && respDor != "Não")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "BLEFARITE OU TERÇOL",
                recommendation = "Compressas mornas 3-4x/dia e higiene palpebral rigorosa"
            });
        }

        // 3. Refractive issues/eye strain (2 diagnoses)
        if (responses.TryGetValue("piora_telas", out var respTelas) && respTelas != "Não" &&
            responses.TryGetValue("olho_seco", out var respSeco) && respSeco != "Não")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "SÍNDROME DO OLHO SECO OU FADIGA VISUAL",
                recommendation = "Pausas a cada 20 minutos (regra 20-20-20) e lágrimas artificiais"
            });
        }

        if (responses.TryGetValue("visao_alterada", out respVisao) && respVisao == "Piorou gradualmente" &&
            responses.TryGetValue("halos_luminosos", out respHalos) && respHalos != "Não")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "ERRO REFRATIVO (MIOPIA/ASTIGMATISMO/HIPERMETROPIA)",
                recommendation = "Avaliação oftalmológica para correção com óculos ou lentes"
            });
        }

        // 4. Systemic conditions (2 diagnoses)
        if (responses.TryGetValue("diabetes", out var respDiabetes) && respDiabetes == "Sim" &&
            responses.TryGetValue("visao_alterada", out respVisao) && respVisao != "Sem alterações")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "RETINOPATIA DIABÉTICA",
                recommendation = "Controle glicêmico rigoroso e avaliação do fundo de olho anual"
            });
        }

        if (responses.TryGetValue("pressao_alta", out var respPressao) && respPressao == "Sim" &&
            responses.TryGetValue("tipo_sintoma", out respSintoma) && respSintoma == "Visão embaçada")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "ALTERAÇÕES VASCULARES RETINIANAS",
                recommendation = "Controle da pressão arterial e avaliação oftalmológica especializada"
            });
        }
    }

    private void DisplayResults()
    {
        // Clear previous diagnoses
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
                diagnosisText.text = $"{i+1}. {diagnoses[i].condition}\n→ {diagnoses[i].recommendation}";
            }
        }
        else
        {
            resultText.text = "🟢 Nenhuma condição específica identificada";
        }

        // Risk classification
        riskLevelText.text = "NÍVEL DE RISCO GERAL:\n" + GetRiskLevelText();
        scoreText.text = $"Pontuação total: {totalScore}/120";
    }

    private string GetRiskLevelText()
    {
        if (totalScore >= 50) return "🚨 RISCO MUITO ELEVADO - Procure ajuda oftalmológica IMEDIATA";
        if (totalScore >= 30) return "⚠️ RISCO MODERADO/ALTO - Agende avaliação em até 24h";
        if (totalScore >= 15) return "🔍 RISCO LEVE - Monitore sintomas e consulte se persistirem";
        return "✅ BAIXO RISCO - Mantenha hábitos de saúde ocular";
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
        string path = Path.Combine(Application.persistentDataPath, "diagnostico_eye.json");
        File.WriteAllText(path, json);
        Debug.Log("Resultados salvos em: " + path);
    }
}