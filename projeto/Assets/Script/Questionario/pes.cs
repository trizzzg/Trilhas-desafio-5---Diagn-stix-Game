using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FeetQuiz : MonoBehaviour
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

    private List<Question> feetQuestions = new List<Question>();
    private int currentQuestionIndex = 0;

    private void Start()
    {
        InitializeFeetQuestions();
    }

    private void InitializeFeetQuestions()
    {
        feetQuestions = new List<Question>
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
                    {"A", ("Calcanhar", 4)},
                    {"B", ("Arco do pé", 3)},
                    {"C", ("Dedos", 3)},
                    {"D", ("Tornozelo", 4)},
                    {"E", ("Todo o pé", 5)},
                    {"F", ("Borda lateral", 3)}
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
                id = "piora_caminhar",
                question = "4. A dor piora ao caminhar ou ficar em pé?",
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
                id = "diabetes",
                question = "8. Você tem diabetes?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, tipo 1 ou 2", 7)},
                    {"B", ("Pré-diabetes", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "circulacao",
                question = "9. Você tem problemas de circulação conhecidos?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim (varizes, trombose)", 6)},
                    {"B", ("Suspeita", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "calçados",
                question = "10. Você usa calçados apertados ou de salto alto?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, diariamente", 5)},
                    {"B", ("Sim, ocasionalmente", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "deformidades",
                question = "11. Você observou deformidades no pé?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim (joanete, dedos em garra)", 5)},
                    {"B", ("Sim (outras)", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "feridas",
                question = "12. Você tem feridas que não cicatrizam?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim", 8)},
                    {"B", ("Não", 0)}
                }
            },
            new Question
            {
                id = "unhas",
                question = "13. Suas unhas estão grossas ou descoloridas?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim", 3)},
                    {"B", ("Não", 0)}
                }
            },
            new Question
            {
                id = "trauma",
                question = "14. Você sofreu algum trauma recente?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim (queda, torção)", 6)},
                    {"B", ("Sim (impacto)", 4)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "atividade",
                question = "15. Pratica atividades de alto impacto (corrida, etc.)?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, frequentemente", 5)},
                    {"B", ("Sim, ocasionalmente", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "rigidez",
                question = "16. Você sente rigidez matinal nas articulações?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, dura mais de 1 hora", 5)},
                    {"B", ("Sim, dura menos de 1 hora", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "febre",
                question = "17. Você tem febre?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, acima 38°C", 5)},
                    {"B", ("Sim, até 38°C", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "historia_familiar",
                question = "18. Há histórico de problemas nos pés na família?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim", 3)},
                    {"B", ("Não", 0)}
                }
            },
            new Question
            {
                id = "peso",
                question = "19. Você está acima do peso?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, obesidade", 5)},
                    {"B", ("Sim, sobrepeso", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "sensibilidade",
                question = "20. Você perdeu a sensibilidade em alguma área do pé?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim", 7)},
                    {"B", ("Não", 0)}
                }
            }
        };
    }

    public void StartFeetQuiz()
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

        if (currentQuestionIndex >= feetQuestions.Count)
        {
            FinishQuiz();
            return;
        }

        Question currentQuestion = feetQuestions[currentQuestionIndex];
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
        if ((responses.TryGetValue("diabetes", out var respDiabetes) && 
             (respDiabetes == "Sim, tipo 1 ou 2" || respDiabetes == "Pré-diabetes")) &&
            responses.TryGetValue("feridas", out var respFeridas) && respFeridas == "Sim")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "PÉ DIABÉTICO COM INFECÇÃO",
                recommendation = "Emergência médica! Risco de amputação - procure atendimento IMEDIATO"
            });
        }
        
        if (responses.TryGetValue("sensibilidade", out var respSensibilidade) && respSensibilidade == "Sim" &&
            responses.TryGetValue("formigamento", out var respFormigamento) && respFormigamento != "Não")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "NEUROPATIA PERIFÉRICA GRAVE",
                recommendation = "Avaliação neurológica urgente necessária"
            });
        }

        if (responses.TryGetValue("inchaço", out var respInchaco) && respInchaco == "Sim, intensa" &&
            responses.TryGetValue("vermelhidão", out var respVermelho) && respVermelho == "Sim" &&
            responses.TryGetValue("febre", out var respFebre) && respFebre != "Não")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "CELULITE INFECCIOSA OU TROMBOSE",
                recommendation = "Procure atendimento URGENTE"
            });
        }

        // 2. Orthopedic problems
        if (responses.TryGetValue("localizacao", out var respLocalizacao) && respLocalizacao == "Calcanhar" &&
            responses.TryGetValue("piora_caminhar", out var respCaminhar) && respCaminhar != "Não")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "FASCITE PLANTAR",
                recommendation = "Alongamentos e palmilhas podem ajudar"
            });
        }

        if (responses.TryGetValue("deformidades", out var respDeformidades) && respDeformidades != "Não" &&
            responses.TryGetValue("calçados", out var respCalcados) && respCalcados != "Não")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "JOANETE (HÁLUX VALGO) OU DEDOS EM GARRA",
                recommendation = "Avaliação ortopédica e calçados adequados"
            });
        }

        if (responses.TryGetValue("localizacao", out respLocalizacao) && respLocalizacao == "Arco do pé" &&
            responses.TryGetValue("atividade", out var respAtividade) && respAtividade != "Não")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "TENDINITE OU ESPORÃO",
                recommendation = "Repouso e gelo local"
            });
        }

        if (responses.TryGetValue("trauma", out var respTrauma) && respTrauma != "Não" &&
            responses.TryGetValue("localizacao", out respLocalizacao) && respLocalizacao == "Tornozelo")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "ENTORSE OU FRATURA",
                recommendation = "Imobilização e avaliação por imagem"
            });
        }

        // 3. Neurological/circulatory problems
        if (responses.TryGetValue("formigamento", out respFormigamento) && respFormigamento != "Não" &&
            responses.TryGetValue("calçados", out respCalcados) && respCalcados != "Não" &&
            (responses.TryGetValue("localizacao", out respLocalizacao) && 
             (respLocalizacao == "Dedos" || respLocalizacao == "Arco do pé")))
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "NEUROMA DE MORTON",
                recommendation = "Calçados mais largos e avaliação podológica"
            });
        }

        if (responses.TryGetValue("circulacao", out var respCirculacao) && respCirculacao != "Não" &&
            responses.TryGetValue("inchaço", out respInchaco) && respInchaco != "Não")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "INSUFICIÊNCIA VENOSA",
                recommendation = "Meias de compressão e elevação das pernas"
            });
        }

        if (responses.TryGetValue("rigidez", out var respRigidez) && respRigidez != "Não" &&
            responses.TryGetValue("historia_familiar", out var respHistoria) && respHistoria == "Sim")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "ARTRITE REUMATOIDE",
                recommendation = "Avaliação reumatológica necessária"
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
        if (totalScore >= 50) return "🚨 RISCO MUITO ELEVADO - Procure ajuda profissional IMEDIATA";
        if (totalScore >= 30) return "⚠️ RISCO MODERADO/ALTO - Agende avaliação médica em até 48h";
        if (totalScore >= 15) return "🔍 RISCO LEVE - Monitore sintomas e consulte se persistirem";
        return "✅ BAIXO RISCO - Mantenha hábitos saudáveis";
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
        string path = Path.Combine(Application.persistentDataPath, "diagnostico_pes.json");
        File.WriteAllText(path, json);
        Debug.Log("Resultados salvos em: " + path);
    }
}