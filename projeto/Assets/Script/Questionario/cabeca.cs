using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class HeadQuiz : MonoBehaviour
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

    private List<Question> headQuestions = new List<Question>();
    private int currentQuestionIndex = 0;

    private void Start()
    {
        InitializeHeadQuestions();
    }

    private void InitializeHeadQuestions()
    {
        headQuestions = new List<Question>
        {
            new Question
            {
                id = "intensidade",
                question = "1. Qual a intensidade da dor?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Leve (não interfere nas atividades)", 1)},
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
                    {"A", ("Em toda a cabeça", 2)},
                    {"B", ("Apenas de um lado", 4)},
                    {"C", ("Na testa ou região dos olhos", 3)},
                    {"D", ("Na nuca", 3)},
                    {"E", ("Em pontos específicos", 2)}
                }
            },
            new Question
            {
                id = "duracao",
                question = "3. Há quanto tempo você está com dor?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Menos de 1 hora", 1)},
                    {"B", ("1 a 24 horas", 3)},
                    {"C", ("1 a 3 dias", 5)},
                    {"D", ("Mais de 3 dias", 7)}
                }
            },
            new Question
            {
                id = "frequencia",
                question = "4. Com que frequência sente dores de cabeça?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Primeira vez", 1)},
                    {"B", ("Menos de 1 vez por mês", 2)},
                    {"C", ("1-3 vezes por mês", 3)},
                    {"D", ("1-2 vezes por semana", 5)},
                    {"E", ("Quase diariamente", 7)}
                }
            },
            new Question
            {
                id = "pulsatil",
                question = "5. A dor é pulsátil (latejante)?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, claramente", 4)},
                    {"B", ("Parcialmente", 2)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "enjoo_vomitos",
                question = "6. Você tem enjoo ou vômitos?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, com vômitos", 6)},
                    {"B", ("Sim, apenas enjoo", 4)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "sensibilidade",
                question = "7. Você tem sensibilidade à luz ou ao barulho?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, a ambos", 5)},
                    {"B", ("Apenas à luz", 3)},
                    {"C", ("Apenas ao barulho", 3)},
                    {"D", ("Não", 0)}
                }
            },
            new Question
            {
                id = "trauma",
                question = "8. Você teve trauma ou batida na cabeça recentemente?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, nos últimos 3 dias", 10)},
                    {"B", ("Sim, mais de 3 dias atrás", 5)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "febre",
                question = "9. Você tem febre?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, acima de 38°C", 7)},
                    {"B", ("Sim, entre 37-38°C", 4)},
                    {"C", ("Não", 0)},
                    {"D", ("Não medi", 1)}
                }
            },
            new Question
            {
                id = "alteracoes_visao",
                question = "10. Você tem alterações na visão?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Visão embaçada ou dupla", 6)},
                    {"B", ("Pontos luminosos ou aura", 5)},
                    {"C", ("Nenhuma alteração", 0)}
                }
            },
            new Question
            {
                id = "dificuldade_fala",
                question = "11. Você tem dificuldade para falar ou se expressar?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, significativa", 8)},
                    {"B", ("Sim, leve", 4)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "fraqueza_membros",
                question = "12. Você sente fraqueza em algum membro?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, em um lado do corpo", 8)},
                    {"B", ("Sim, em ambos os lados", 5)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "perda_olfato",
                question = "13. Você perdeu o olfato ou paladar subitamente?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, totalmente", 6)},
                    {"B", ("Sim, parcialmente", 4)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "rigidez_pescoco",
                question = "14. Você sente rigidez no pescoço ou dor ao movimentar a cabeça?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, muita dificuldade", 7)},
                    {"B", ("Sim, um pouco", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "convulsoes",
                question = "15. Você teve convulsões ou desmaios recentes?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, convulsão", 15)},
                    {"B", ("Sim, desmaio", 8)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "uso_medicamentos",
                question = "16. Você usa algum medicamento para dor de cabeça?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, frequentemente", 5)},
                    {"B", ("Sim, ocasionalmente", 2)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "historia_familiar",
                question = "17. Há histórico de enxaqueca ou problemas neurológicos na família?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, em parentes próximos", 3)},
                    {"B", ("Não sei", 1)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "piora_esforco",
                question = "18. A dor piora com esforço físico ou tosse?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, significativamente", 6)},
                    {"B", ("Sim, levemente", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "pressao_alta",
                question = "19. Você tem ou suspeita ter pressão alta?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, diagnosticada", 5)},
                    {"B", ("Suspeito que sim", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "estresse",
                question = "20. Como você avalia seu nível de estresse recente?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Muito alto", 4)},
                    {"B", ("Moderado", 2)},
                    {"C", ("Normal", 0)},
                    {"D", ("Baixo", 0)}
                }
            }
        };
    }


    public void StartHeadQuiz()
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

        if (currentQuestionIndex >= headQuestions.Count)
        {
            FinishQuiz();
            return;
        }

        Question currentQuestion = headQuestions[currentQuestionIndex];
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

        // 1. Emergências (prioridade máxima)
        if (responses.TryGetValue("trauma", out var respTrauma) && 
            (respTrauma == "Sim, nos últimos 3 dias" || respTrauma == "Sim, mais de 3 dias atrás"))
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "EMERGÊNCIA NEUROLÓGICA",
                recommendation = "Procure atendimento médico IMEDIATO (pode indicar AVC, hemorragia ou trauma grave)"
            });
        }

        if (responses.TryGetValue("alteracoes_visao", out var respVisao) && respVisao == "Visão embaçada ou dupla")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "ALTERAÇÕES VISUAIS GRAVES",
                recommendation = "Pode indicar problema neurológico - avaliação urgente necessária"
            });
        }

        if (responses.TryGetValue("febre", out var respFebre) && 
            (respFebre == "Sim, acima de 38°C" || respFebre == "Sim, entre 37-38°C") &&
            responses.TryGetValue("rigidez_pescoco", out var respPescoco) &&
            (respPescoco == "Sim, muita dificuldade" || respPescoco == "Sim, um pouco"))
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "POSSÍVEL MENINGITE",
                recommendation = "Procure atendimento URGENTE (risco de infecção grave)"
            });
        }

        // 2. Enxaqueca
        if (responses.TryGetValue("pulsatil", out var respPulsatil) && respPulsatil == "Sim, claramente" &&
            responses.TryGetValue("sensibilidade", out var respSensibilidade) &&
            (respSensibilidade == "Sim, a ambos" || respSensibilidade == "Apenas à luz") &&
            responses.TryGetValue("localizacao", out var respLocalizacao) && respLocalizacao == "Apenas de um lado" &&
            responses.TryGetValue("alteracoes_visao", out respVisao) && respVisao == "Pontos luminosos ou aura")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "ENXAQUECA COM AURA",
                recommendation = "Consulte neurologista para tratamento preventivo"
            });
        }
        else if (responses.TryGetValue("pulsatil", out respPulsatil) &&
                 (respPulsatil == "Sim, claramente" || respPulsatil == "Parcialmente") &&
                 responses.TryGetValue("localizacao", out respLocalizacao) && respLocalizacao == "Apenas de um lado" &&
                 responses.TryGetValue("sensibilidade", out respSensibilidade) && respSensibilidade != "Não")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "ENXAQUECA",
                recommendation = "Evite gatilhos como estresse, certos alimentos e privação de sono"
            });
        }

        // 3. Cefaleia tensional
        if (responses.TryGetValue("localizacao", out respLocalizacao) &&
            (respLocalizacao == "Em toda a cabeça" || respLocalizacao == "Na nuca") &&
            responses.TryGetValue("pulsatil", out respPulsatil) && respPulsatil == "Não" &&
            responses.TryGetValue("estresse", out var respEstresse) &&
            (respEstresse == "Muito alto" || respEstresse == "Moderado"))
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "CEFALEIA TENSIONAL",
                recommendation = "Técnicas de relaxamento, massagem e controle do estresse"
            });
        }

        // 4. Sinusite
        if (responses.TryGetValue("localizacao", out respLocalizacao) && respLocalizacao == "Na testa ou região dos olhos" &&
            responses.TryGetValue("febre", out respFebre) && respFebre != "Não" &&
            responses.TryGetValue("duracao", out var respDuracao) &&
            (respDuracao == "1 a 3 dias" || respDuracao == "Mais de 3 dias"))
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "POSSÍVEL SINUSITE",
                recommendation = "Consulte médico para avaliação de infecção sinusal"
            });
        }

        // 5. COVID-19
        if (responses.TryGetValue("perda_olfato", out var respOlfato) && respOlfato != "Não")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "POSSÍVEL COVID-19",
                recommendation = "Isole-se e faça teste PCR ou antígeno"
            });
        }

        // 6. Cefaleia por uso excessivo de medicamentos
        if (responses.TryGetValue("uso_medicamentos", out var respMedicamentos) &&
            (respMedicamentos == "Sim, frequentemente" || respMedicamentos == "Sim, ocasionalmente") &&
            responses.TryGetValue("frequencia", out var respFrequencia) &&
            (respFrequencia == "1-2 vezes por semana" || respFrequencia == "Quase diariamente"))
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "CEFALEIA POR USO EXCESSIVO DE ANALGÉSICOS",
                recommendation = "Reduza gradualmente o uso de analgésicos com orientação médica"
            });
        }

        // 7. Hipertensão
        if (responses.TryGetValue("pressao_alta", out var respPressao) && respPressao != "Não" &&
            responses.TryGetValue("localizacao", out respLocalizacao) && respLocalizacao == "Em toda a cabeça" &&
            responses.TryGetValue("intensidade", out var respIntensidade) &&
            (respIntensidade == "Moderada (atrapalha atividades)" || respIntensidade == "Forte (incapacitante)"))
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "CEFALEIA POR HIPERTENSÃO",
                recommendation = "Monitore sua pressão arterial e consulte um cardiologista"
            });
        }

        // 8. Cefaleia em salvas
        if (responses.TryGetValue("intensidade", out respIntensidade) && respIntensidade == "A pior dor que já senti" &&
            responses.TryGetValue("localizacao", out respLocalizacao) && respLocalizacao == "Apenas de um lado" &&
            responses.TryGetValue("duracao", out respDuracao) &&
            (respDuracao == "Menos de 1 hora" || respDuracao == "1 a 24 horas") &&
            responses.TryGetValue("frequencia", out respFrequencia) &&
            (respFrequencia == "1-3 vezes por mês" || respFrequencia == "1-2 vezes por semana"))
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "CEFALEIA EM SALVAS",
                recommendation = "Consulte neurologista - requer tratamento específico"
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
            resultText.text = "Nenhuma condição específica identificada";
        }

        riskLevelText.text = "NÍVEL DE RISCO GERAL:\n" + GetRiskLevelText();
        scoreText.text = $"Pontuação total: {totalScore}/120";
    }

    private string GetRiskLevelText()
    {
        if (totalScore >= 50) return "RISCO MUITO ELEVADO - Procure ajuda profissional IMEDIATA";
        if (totalScore >= 30) return "RISCO MODERADO/ALTO - Agende avaliação médica em até 48h";
        if (totalScore >= 15) return "RISCO LEVE - Monitore sintomas e consulte se persistirem";
        return "BAIXO RISCO - Mantenha hábitos saudáveis";
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
        string path = Path.Combine(Application.persistentDataPath, "diagnostico_head.json");
        File.WriteAllText(path, json);
        Debug.Log("Resultados salvos em: " + path);
    }
}