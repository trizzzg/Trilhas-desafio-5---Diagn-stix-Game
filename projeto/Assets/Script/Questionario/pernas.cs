using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LegsQuiz : MonoBehaviour
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

    private List<Question> legsQuestions = new List<Question>();
    private int currentQuestionIndex = 0;

    private void Start()
    {
        InitializeLegsQuestions();
    }

    private void InitializeLegsQuestions()
    {
        legsQuestions = new List<Question>
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
                    {"A", ("Coxa", 3)},
                    {"B", ("Joelho", 4)},
                    {"C", ("Panturrilha", 4)},
                    {"D", ("Tornozelo/Pé", 3)},
                    {"E", ("Toda a perna", 5)},
                    {"F", ("Virilha", 4)}
                }
            },
            new Question
            {
                id = "duracao",
                question = "3. Há quanto tempo você está com dor?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Menos de 1 dia", 1)},
                    {"B", ("1 a 3 dias", 3)},
                    {"C", ("4 a 7 dias", 5)},
                    {"D", ("Mais de 1 semana", 7)}
                }
            },
            new Question
            {
                id = "piora_movimento",
                question = "4. A dor piora ao caminhar ou movimentar a perna?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, significativamente", 5)},
                    {"B", ("Sim, levemente", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "inchaço",
                question = "5. Você notou inchaço na região dolorida?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, muito inchaço", 6)},
                    {"B", ("Sim, leve inchaço", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "vermelhidão",
                question = "6. Há vermelhidão ou calor local?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, intenso", 5)},
                    {"B", ("Sim, moderado", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "formigamento",
                question = "7. Você sente formigamento ou dormência?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, constante", 6)},
                    {"B", ("Sim, intermitente", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "trauma",
                question = "8. Você teve algum trauma ou lesão recente?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, trauma grave", 8)},
                    {"B", ("Sim, pequena lesão", 4)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "varizes",
                question = "9. Você tem varizes ou problemas de circulação conhecidos?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, diagnosticado", 5)},
                    {"B", ("Suspeito que sim", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "perna_pesada",
                question = "10. Você sente a perna pesada ou cansada com frequência?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, diariamente", 5)},
                    {"B", ("Sim, ocasionalmente", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "febre",
                question = "11. Você tem febre?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, acima de 38°C", 7)},
                    {"B", ("Sim, entre 37-38°C", 4)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "pele_mudancas",
                question = "12. Notou mudanças na pele (cor, textura, feridas)?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, feridas ou úlceras", 8)},
                    {"B", ("Sim, escurecimento", 4)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "dor_repouso",
                question = "13. A dor aparece mesmo em repouso?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, frequentemente", 6)},
                    {"B", ("Às vezes", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "caimbras",
                question = "14. Você tem câimbras frequentes?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, diariamente", 5)},
                    {"B", ("Sim, ocasionalmente", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "pulsacao",
                question = "15. Consegue sentir o pulso no pé da perna dolorida?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, normal", 0)},
                    {"B", ("Difícil de sentir", 5)},
                    {"C", ("Não sinto", 8)}
                }
            },
            new Question
            {
                id = "edema",
                question = "16. Ao pressionar o inchaço, fica marca do dedo?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, marca persistente", 6)},
                    {"B", ("Sim, marca leve", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "historia_trombose",
                question = "17. Já teve trombose ou embolia pulmonar?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim", 10)},
                    {"B", ("Não sei", 2)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "viagem_recente",
                question = "18. Fez viagem longa (>4h) recentemente?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, de avião", 6)},
                    {"B", ("Sim, de carro/ônibus", 4)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "imobilizacao",
                question = "19. Esteve imobilizado ou acamado recentemente?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, mais de 3 dias", 8)},
                    {"B", ("Sim, 1-3 dias", 4)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "hormonios",
                question = "20. Usa anticoncepcional ou terapia hormonal?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim", 5)},
                    {"B", ("Não", 0)}
                }
            }
        };
    }

    public void StartLegsQuiz()
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

        if (currentQuestionIndex >= legsQuestions.Count)
        {
            FinishQuiz();
            return;
        }

        Question currentQuestion = legsQuestions[currentQuestionIndex];
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
        if (responses.TryGetValue("inchaço", out var respInchaco) && 
            (respInchaco == "Sim, muito inchaço" || respInchaco == "Sim, leve inchaço") &&
            responses.TryGetValue("localizacao", out var respLocalizacao) && respLocalizacao == "Panturrilha" &&
            responses.TryGetValue("historia_trombose", out var respTrombose) && respTrombose == "Sim")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "TROMBOSE VENOSA PROFUNDA (TVP)",
                recommendation = "Emergência médica! Risco de embolia pulmonar. Procure atendimento IMEDIATO"
            });
        }

        if (responses.TryGetValue("pulsacao", out var respPulsacao) && 
            (respPulsacao == "Difícil de sentir" || respPulsacao == "Não sinto"))
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "ISQUEMIA ARTERIAL",
                recommendation = "Procure atendimento URGENTE - risco de perda do membro"
            });
        }

        // 2. Vascular problems
        if (responses.TryGetValue("varizes", out var respVarizes) && respVarizes != "Não" &&
            responses.TryGetValue("perna_pesada", out var respPernaPesada) && respPernaPesada != "Não" &&
            responses.TryGetValue("edema", out var respEdema) && respEdema != "Não")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "INSUFICIÊNCIA VENOSA CRÔNICA",
                recommendation = "Use meias de compressão e eleve as pernas"
            });
        }

        // 3. Joint problems
        if (responses.TryGetValue("localizacao", out respLocalizacao) && respLocalizacao == "Joelho" &&
            responses.TryGetValue("inchaço", out respInchaco) && respInchaco != "Não" &&
            responses.TryGetValue("piora_movimento", out var respMovimento) && respMovimento != "Não")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "PROBLEMA ARTICULAR (ARTROSE/LESÃO)",
                recommendation = "Avaliação ortopédica recomendada"
            });
        }

        // 4. Neuropathies
        if (responses.TryGetValue("formigamento", out var respFormigamento) && respFormigamento != "Não" &&
            responses.TryGetValue("localizacao", out respLocalizacao) && respLocalizacao == "Toda a perna")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "COMPRESSÃO NERVOSA OU NEUROPATIA",
                recommendation = "Pode ser hérnia de disco ou diabetes - avalie com neurologista"
            });
        }

        // 5. Infections
        if (responses.TryGetValue("febre", out var respFebre) && respFebre != "Não" &&
            responses.TryGetValue("vermelhidão", out var respVermelhidão) && respVermelhidão != "Não")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "INFECÇÃO (CELULITE/ERISIPELA)",
                recommendation = "Pode requerer antibióticos - consulte um médico"
            });
        }

        // 6. Muscle problems
        if (responses.TryGetValue("trauma", out var respTrauma) && respTrauma != "Não" &&
            responses.TryGetValue("piora_movimento", out respMovimento) && respMovimento != "Não")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "LESÃO MUSCULAR",
                recommendation = "Repouso, gelo e anti-inflamatórios podem ajudar"
            });
        }

        // 7. Intermittent claudication
        if (responses.TryGetValue("dor_repouso", out var respDorRepouso) && respDorRepouso == "Sim, frequentemente" &&
            responses.TryGetValue("piora_movimento", out respMovimento) && respMovimento != "Não")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "DOENÇA ARTERIAL PERIFÉRICA",
                recommendation = "Avaliação vascular necessária"
            });
        }

        // 8. Risk factors for DVT
        if ((responses.TryGetValue("viagem_recente", out var respViagem) && respViagem != "Não") ||
            (responses.TryGetValue("imobilizacao", out var respImobilizacao) && respImobilizacao != "Não") ||
            (responses.TryGetValue("hormonios", out var respHormonios) && respHormonios == "Sim"))
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "RISCO ELEVADO PARA TROMBOSE",
                recommendation = "Mantenha-se hidratado e mexa-se regularmente"
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
        if (totalScore >= 60) return "🚨 RISCO MUITO ELEVADO - Procure ajuda profissional IMEDIATA";
        if (totalScore >= 35) return "⚠️ RISCO MODERADO/ALTO - Agende avaliação médica em até 1 semana";
        if (totalScore >= 15) return "🔍 RISCO LEVE - Monitore sintomas e consulte se persistirem";
        return "✅ BAIXO RISCO - Mantenha hábitos saudáveis";
    }

    private string GetRiskLevel()
    {
        if (totalScore >= 60) return "MUITO ELEVADO";
        if (totalScore >= 35) return "ALTO";
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
        string path = Path.Combine(Application.persistentDataPath, "diagnostico_pernas.json");
        File.WriteAllText(path, json);
        Debug.Log("Resultados salvos em: " + path);
    }
}