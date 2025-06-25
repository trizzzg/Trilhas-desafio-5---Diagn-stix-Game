using System;
using System.Collections.Generic;
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
            button.onClick.AddListener(() => OnOptionSelected(option.Key, currentQuestion));
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
        // 1. Emergências oculares (3 diagnósticos)
        if (responses.ContainsKey("visao_alterada") && responses["visao_alterada"] == "Piorou abruptamente" &&
            responses.ContainsKey("dor_profunda") && (responses["dor_profunda"] == "Sim, intensa" || responses["dor_profunda"] == "Sim, moderada"))
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "GLAUCOMA AGUDO",
                recommendation = "Emergência médica! Risco de perda visual permanente - procure atendimento IMEDIATO"
            });
        }
        
        if ((responses.ContainsKey("trauma") && (responses["trauma"] == "Sim, com objeto pontiagudo" || responses["trauma"] == "Sim, com impacto")) ||
            (responses.ContainsKey("quimicos") && responses["quimicos"] == "Sim"))
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "TRAUMA OU QUEIMADURA OCULAR",
                recommendation = "Lave com água corrente e procure oftalmologista URGENTEMENTE"
            });
        }

        if (responses.ContainsKey("visao_alterada") && responses["visao_alterada"] == "Piorou abruptamente" &&
            responses.ContainsKey("fotofobia") && responses["fotofobia"] == "Sim, não consigo abrir os olhos" &&
            responses.ContainsKey("cefaleia") && responses["cefaleia"] == "Sim, com náuseas")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "UVELITE AGUDA",
                recommendation = "Inflamação intraocular grave - tratamento urgente necessário"
            });
        }

        // 2. Infecções/inflamações (3 diagnósticos)
        if (responses.ContainsKey("secrecao") && responses["secrecao"] == "Sim, purulenta (amarela/esverdeada)" &&
            responses.ContainsKey("inchaço_palpebras") && responses["inchaço_palpebras"] != "Não")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "CONJUNTIVITE BACTERIANA",
                recommendation = "Requer colírios antibióticos - evite automedicação"
            });
        }

        if (responses.ContainsKey("tipo_sintoma") && responses["tipo_sintoma"] == "Coceira" &&
            responses.ContainsKey("secrecao") && responses["secrecao"] == "Sim, aquosa/transparente")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "CONJUNTIVITE ALÉRGICA",
                recommendation = "Anti-histamínicos oculares podem ajudar - evite coçar"
            });
        }

        if (responses.ContainsKey("inchaço_palpebras") && responses["inchaço_palpebras"] == "Sim, com vermelhidão" &&
            responses.ContainsKey("dor_profunda") && responses["dor_profunda"] != "Não")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "BLEFARITE OU TERÇOL",
                recommendation = "Compressas mornas e higiene palpebral"
            });
        }

        // 3. Problemas de refração/fadiga (2 diagnósticos)
        if (responses.ContainsKey("piora_telas") && responses["piora_telas"] != "Não" &&
            responses.ContainsKey("olho_seco") && responses["olho_seco"] != "Não" &&
            responses.ContainsKey("lentes_contato") && responses["lentes_contato"] != "Não")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "SÍNDROME DO OLHO SECO/FADIGA VISUAL",
                recommendation = "Pausas regulares e lágrimas artificiais"
            });
        }

        if (responses.ContainsKey("visao_alterada") && responses["visao_alterada"] == "Piorou gradualmente" &&
            responses.ContainsKey("halos_luminosos") && responses["halos_luminosos"] != "Não")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "ERRO REFRATIVO (MIOPIA/ASTIGMATISMO)",
                recommendation = "Avaliação oftalmológica para correção"
            });
        }

        // 4. Condições sistêmicas (2 diagnósticos)
        if (responses.ContainsKey("diabetes") && responses["diabetes"] == "Sim" &&
            responses.ContainsKey("visao_alterada") && responses["visao_alterada"] != "Sem alterações")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "RETINOPATIA DIABÉTICA",
                recommendation = "Controle glicêmico e avaliação do fundo de olho"
            });
        }

        if (responses.ContainsKey("pressao_alta") && responses["pressao_alta"] == "Sim" &&
            responses.ContainsKey("tipo_sintoma") && responses["tipo_sintoma"] == "Visão embaçada")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "ALTERAÇÕES VASCULARES RETINIANAS",
                recommendation = "Monitoramento da pressão e avaliação oftalmológica"
            });
        }
    }

    private void DisplayResults()
{
    Debug.Log("=== DisplayResults chamado ===");
    Debug.Log("diagnosesContainer: " + diagnosesContainer);
    Debug.Log("diagnosisPrefab: " + diagnosisPrefab);
    Debug.Log("resultText: " + resultText);
    Debug.Log("scoreText: " + scoreText);
    Debug.Log("riskLevelText: " + riskLevelText);

    if (diagnosesContainer == null)
    {
        Debug.LogError("❌ diagnosesContainer NÃO FOI ATRIBUÍDO NO INSPECTOR");
        return;
    }

    if (diagnosisPrefab == null)
    {
        Debug.LogError("❌ diagnosisPrefab NÃO FOI ATRIBUÍDO NO INSPECTOR");
        return;
    }

    if (resultText == null || scoreText == null || riskLevelText == null)
    {
        Debug.LogError("❌ Um dos TextMeshProUGUI (resultText, scoreText ou riskLevelText) está NULL");
        return;
    }
        // Clear previous diagnoses
        foreach (Transform child in diagnosesContainer)
        {
            Destroy(child.gameObject);
        }

        if (diagnoses.Count > 0)
        {
            resultText.text = "🔍 DIAGNÓSTICOS IDENTIFICADOS (10 possibilidades):";
            for (int i = 0; i < diagnoses.Count; i++)
            {
                GameObject diagnosisObj = Instantiate(diagnosisPrefab, diagnosesContainer);
                TMPro.TextMeshProUGUI diagnosisText = diagnosisObj.GetComponent<TMPro.TextMeshProUGUI>();
                diagnosisText.text = $"{i+1}. {diagnoses[i].condition}\n→ {diagnoses[i].recommendation}";
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
            riskLevelText.text += "RISCO MUITO ELEVADO - Procure ajuda oftalmológica IMEDIATA";
        }
        else if (totalScore >= 30)
        {
            riskLevelText.text += "RISCO MODERADO/ALTO - Agende avaliação em até 24h";
        }
        else if (totalScore >= 15)
        {
            riskLevelText.text += "RISCO LEVE - Monitore sintomas e consulte se persistirem";
        }
        else
        {
            riskLevelText.text += "BAIXO RISCO - Mantenha hábitos de saúde ocular";
        }

        scoreText.text = $"Pontuação total: {totalScore}/120";
    }
}