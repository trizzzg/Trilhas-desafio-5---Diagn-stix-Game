using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ArmsQuiz : MonoBehaviour
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

    private List<Question> armsQuestions = new List<Question>();
    private int currentQuestionIndex = 0;

    private void Start()
    {
        InitializeArmsQuestions();
    }

    private void InitializeArmsQuestions()
    {
        armsQuestions = new List<Question>
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
                    {"A", ("Ombro", 4)},
                    {"B", ("Cotovelo", 3)},
                    {"C", ("Punho/Mão", 3)},
                    {"D", ("Braço inteiro", 5)},
                    {"E", ("Parte superior do braço", 3)},
                    {"F", ("Parte inferior do braço", 2)}
                }
            },
            new Question
            {
                id = "duracao",
                question = "3. Há quanto tempo você está com dor?",
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
                id = "piora_movimento",
                question = "4. A dor piora ao movimentar o braço?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, muito", 5)},
                    {"B", ("Sim, pouco", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "inchaço",
                question = "5. Você notou inchaço no local?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, intensa", 5)},
                    {"B", ("Sim, leve", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "vermelhidão",
                question = "6. Há vermelhidão ou calor local?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim", 4)},
                    {"B", ("Não", 0)}
                }
            },
            new Question
            {
                id = "formigamento",
                question = "7. Você sente formigamento ou dormência?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, constante", 6)},
                    {"B", ("Sim, intermitente", 4)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "trauma",
                question = "8. Você teve algum trauma recente?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, trauma grave", 8)},
                    {"B", ("Sim, pequena lesão", 4)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "esforco_repetitivo",
                question = "9. Você realiza movimentos repetitivos no trabalho/lazer?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, frequentemente", 5)},
                    {"B", ("Sim, ocasionalmente", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "fraqueza",
                question = "10. Você tem dificuldade para segurar objetos ou fraqueza no braço?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, intensa", 6)},
                    {"B", ("Sim, leve", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "irradiacao",
                question = "11. A dor irradia do pescoço para o braço?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim", 5)},
                    {"B", ("Não", 0)}
                }
            },
            new Question
            {
                id = "posicao_dor",
                question = "12. A dor piora em determinadas posições?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, à noite", 4)},
                    {"B", ("Sim, ao levantar o braço", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "rigidez",
                question = "13. Você sente rigidez articular?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, intensa", 5)},
                    {"B", ("Sim, leve", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "febre",
                question = "14. Você tem febre?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, acima 38°C", 5)},
                    {"B", ("Sim, até 38°C", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "historia_artrite",
                question = "15. Você tem histórico de artrite ou problemas reumáticos?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim", 4)},
                    {"B", ("Não", 0)}
                }
            },
            new Question
            {
                id = "diabetes",
                question = "16. Você tem diabetes?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim", 3)},
                    {"B", ("Não", 0)}
                }
            },
            new Question
            {
                id = "edema_dedos",
                question = "17. Você notou inchaço nos dedos?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim", 3)},
                    {"B", ("Não", 0)}
                }
            },
            new Question
            {
                id = "alteracoes_pele",
                question = "18. Há alterações na pele (cor, textura)?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim", 3)},
                    {"B", ("Não", 0)}
                }
            },
            new Question
            {
                id = "uso_medicamentos",
                question = "19. Você usa medicamentos para dor?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, frequentemente", 4)},
                    {"B", ("Sim, ocasionalmente", 2)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "estresse",
                question = "20. Você está passando por período de estresse?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, intenso", 3)},
                    {"B", ("Sim, moderado", 2)},
                    {"C", ("Não", 0)}
                }
            }
        };
    }


    public void StartArmsQuiz()
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

        if (currentQuestionIndex >= armsQuestions.Count)
        {
            FinishQuiz();
            return;
        }

        Question currentQuestion = armsQuestions[currentQuestionIndex];
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

        if (responses.TryGetValue("trauma", out var respTrauma) && respTrauma == "Sim, trauma grave" &&
            responses.TryGetValue("fraqueza", out var respFraqueza) && respFraqueza == "Sim, intensa")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "FRATURA OU LUXAÇÃO",
                recommendation = "Emergência ortopédica! Imobilize e procure atendimento IMEDIATO"
            });
        }

        if (responses.TryGetValue("inchaço", out var respInchaco) && respInchaco == "Sim, intensa" &&
            responses.TryGetValue("vermelhidão", out var respVermelho) && respVermelho == "Sim" &&
            responses.TryGetValue("febre", out var respFebre) && respFebre != "Não")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "INFECÇÃO OU TROMBOSE",
                recommendation = "Procure atendimento URGENTE"
            });
        }

        if (responses.TryGetValue("irradiacao", out var respIrradiacao) && respIrradiacao == "Sim" &&
            responses.TryGetValue("formigamento", out var respFormigamento) && respFormigamento != "Não")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "COMPRESSÃO NERVOSA CERVICAL",
                recommendation = "Pode ser hérnia de disco - avaliação com neurologista"
            });
        }

        if (responses.TryGetValue("localizacao", out var respLocalizacao) &&
            (respLocalizacao == "Ombro" || respLocalizacao == "Cotovelo") &&
            responses.TryGetValue("piora_movimento", out var respMovimento) && respMovimento != "Não" &&
            responses.TryGetValue("esforco_repetitivo", out var respRepetitivo) && respRepetitivo != "Não")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "TENDINITE/BURSITE",
                recommendation = "Repouso, gelo e fisioterapia podem ajudar"
            });
        }

        if (responses.TryGetValue("localizacao", out respLocalizacao) && respLocalizacao == "Punho/Mão" &&
            responses.TryGetValue("formigamento", out respFormigamento) && respFormigamento != "Não" &&
            responses.TryGetValue("posicao_dor", out var respPosicao) && respPosicao == "Sim, à noite")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "SÍNDROME DO TÚNEL DO CARPO",
                recommendation = "Uso de tala noturna pode ajudar - avaliação ortopédica"
            });
        }

        if (responses.TryGetValue("localizacao", out respLocalizacao) && respLocalizacao == "Cotovelo" &&
            responses.TryGetValue("esforco_repetitivo", out respRepetitivo) && respRepetitivo != "Não")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "EPICONDILITE (COTOVELO DE TENISTA)",
                recommendation = "Modificação de atividades e exercícios específicos"
            });
        }

        if (responses.TryGetValue("historia_artrite", out var respArtrite) && respArtrite == "Sim" &&
            responses.TryGetValue("rigidez", out var respRigidez) && respRigidez != "Não" &&
            responses.TryGetValue("edema_dedos", out var respEdema) && respEdema == "Sim")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "ARTRITE REUMATOIDE",
                recommendation = "Avaliação reumatológica necessária"
            });
        }

        if (responses.TryGetValue("alteracoes_pele", out var respPele) && respPele == "Sim" &&
            responses.TryGetValue("diabetes", out var respDiabetes) && respDiabetes == "Sim")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "PROBLEMAS CIRCULATÓRIOS",
                recommendation = "Avaliação vascular recomendada"
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
            resultText.text = "🔍DIAGNÓSTICOS IDENTIFICADOS:";
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
        if (totalScore >= 30) return "RISCO MODERADO/ALTO - Agende avaliação médica em até 1 semana";
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
                userId = "usr123",  //adicionar UsuarioLogado.userId quando tiver o login
                username = "João Silva" //adicionar UsuarioLogado.username quando tiver o login
            },
            responses = responses,
            totalScore = totalScore,
            diagnoses = diagnoses,
            riskLevel = GetRiskLevel(),
            timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };

        string json = JsonUtility.ToJson(result, true);
        string path = Path.Combine(Application.persistentDataPath, "diagnostico_arms.json");
        File.WriteAllText(path, json);
        Debug.Log("Resultados salvos em: " + path);
    }
}
