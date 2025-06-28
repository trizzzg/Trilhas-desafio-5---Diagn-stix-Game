using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EarsQuiz : MonoBehaviour
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

    private List<Question> earsQuestions = new List<Question>();
    private int currentQuestionIndex = 0;

    private void Start()
    {
        InitializeEarsQuestions();
    }

    private void InitializeEarsQuestions()
    {
        earsQuestions = new List<Question>
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
                    {"D", ("Extrema (insuportável)", 8)}
                }
            },
            new Question
            {
                id = "tipo_sintoma",
                question = "2. Qual o principal sintoma?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Dor", 4)},
                    {"B", ("Zumbido constante", 5)},
                    {"C", ("Diminuição da audição", 6)},
                    {"D", ("Coceira", 2)},
                    {"E", ("Secreção", 4)},
                    {"F", ("Sensação de ouvido tampado", 3)}
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
                id = "piora_toque",
                question = "4. Os sintomas pioram ao tocar ou movimentar a orelha?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, muito", 5)},
                    {"B", ("Sim, pouco", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "secrecao",
                question = "5. Você notou secreção no ouvido?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, purulenta (amarela/esverdeada)", 6)},
                    {"B", ("Sim, aquosa/transparente", 3)},
                    {"C", ("Sim, com sangue", 8)},
                    {"D", ("Não", 0)}
                }
            },
            new Question
            {
                id = "febre",
                question = "6. Você teve febre recentemente?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, acima 38°C", 5)},
                    {"B", ("Sim, até 38°C", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "atividades_aquaticas",
                question = "7. Você nadou, mergulhou ou teve contato com água recentemente?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, mergulho profundo", 6)},
                    {"B", ("Sim, natação superficial", 4)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "tontura",
                question = "8. Você sente tontura ou vertigem?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, com náuseas/vômitos", 7)},
                    {"B", ("Sim, leve", 4)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "perda_auditiva",
                question = "9. Como descreveria a perda auditiva?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Súbita e intensa", 9)},
                    {"B", ("Progressiva", 5)},
                    {"C", ("Flutuante", 4)},
                    {"D", ("Sem perda", 0)}
                }
            },
            new Question
            {
                id = "barotrauma",
                question = "10. Esteve em avião ou locais de altitude recentemente?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, com dor intensa", 6)},
                    {"B", ("Sim, sem dor", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "trauma",
                question = "11. Sofreu trauma no ouvido ou cabeça?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, com objeto pontiagudo", 8)},
                    {"B", ("Sim, com impacto", 5)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "uso_cotonete",
                question = "12. Você usa cotonetes ou objetos para limpar o ouvido?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, frequentemente", 5)},
                    {"B", ("Sim, ocasionalmente", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "zumbido_caracteristica",
                question = "13. O zumbido é pulsátil (parece batimento cardíaco)?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim", 6)},
                    {"B", ("Não", 0)}
                }
            },
            new Question
            {
                id = "dor_mandibular",
                question = "14. Você sente dor na mandíbula ou ao mastigar?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim", 4)},
                    {"B", ("Não", 0)}
                }
            },
            new Question
            {
                id = "historia_infeccoes",
                question = "15. Tem histórico de infecções de ouvido recorrentes?",
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
                id = "pressao_alta",
                question = "17. Você tem hipertensão arterial?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim", 3)},
                    {"B", ("Não", 0)}
                }
            },
            new Question
            {
                id = "uso_medicamentos",
                question = "18. Usou medicamentos ototóxicos recentemente?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim (antibióticos, diuréticos, etc.)", 5)},
                    {"B", ("Não", 0)}
                }
            },
            new Question
            {
                id = "exposicao_ruido",
                question = "19. Ficou exposto a ruídos altos recentemente?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, prolongado", 5)},
                    {"B", ("Sim, breve", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "cefaleia",
                question = "20. Você está com dor de cabeça intensa?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, com náuseas", 5)},
                    {"B", ("Sim, sem náuseas", 3)},
                    {"C", ("Não", 0)}
                }
            }
        };
    }

    public void StartEarsQuiz()
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

        if (currentQuestionIndex >= earsQuestions.Count)
        {
            FinishQuiz();
            return;
        }

        Question currentQuestion = earsQuestions[currentQuestionIndex];
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
        if ((responses.TryGetValue("perda_auditiva", out var respPerda) && respPerda == "Súbita e intensa") ||
            (responses.TryGetValue("tipo_sintoma", out var respTipo) && respTipo == "Zumbido constante" &&
             responses.TryGetValue("perda_auditiva", out respPerda) && respPerda != "Sem perda"))
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "PERDA AUDITIVA SÚBITA",
                recommendation = "Emergência médica! Procure otorrinolaringologista em até 24h"
            });
        }
        
        if ((responses.TryGetValue("secrecao", out var respSecrecao) && respSecrecao == "Sim, com sangue") ||
            (responses.TryGetValue("trauma", out var respTrauma) && respTrauma == "Sim, com objeto pontiagudo"))
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "TRAUMA DO OUVIDO/TÍMPANO PERFURADO",
                recommendation = "Não manipule o ouvido - procure atendimento URGENTE"
            });
        }

        if (responses.TryGetValue("tontura", out var respTontura) && respTontura == "Sim, com náuseas/vômitos" &&
            responses.TryGetValue("cefaleia", out var respCefaleia) && respCefaleia == "Sim, com náuseas")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "LABIRINTITE GRAVE OU NEURITE VESTIBULAR",
                recommendation = "Repouso e avaliação otoneurológica urgente"
            });
        }

        // 2. Infections (3 diagnoses)
        if (responses.TryGetValue("febre", out var respFebre) && respFebre != "Não" &&
            responses.TryGetValue("piora_toque", out var respPiora) && respPiora != "Não" &&
            responses.TryGetValue("intensidade", out var respIntensidade) && 
            (respIntensidade == "Forte (dor incapacitante)" || respIntensidade == "Extrema (insuportável)"))
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "OTITE MÉDIA AGUDA",
                recommendation = "Pode requerer antibióticos - evite automedicação"
            });
        }

        if (responses.TryGetValue("atividades_aquaticas", out var respAgua) && respAgua != "Não" &&
            responses.TryGetValue("secrecao", out respSecrecao) && 
            (respSecrecao == "Sim, purulenta (amarela/esverdeada)" || respSecrecao == "Sim, aquosa/transparente"))
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "OTITE EXTERNA (OUVIDO DE NADADOR)",
                recommendation = "Evite água e use calor local - pode precisar de antibióticos tópicos"
            });
        }

        if (responses.TryGetValue("secrecao", out respSecrecao) && respSecrecao == "Sim, purulenta (amarela/esverdeada)" &&
            responses.TryGetValue("historia_infeccoes", out var respHistoria) && respHistoria == "Sim")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "OTITE MÉDIA CRÔNICA SUPURATIVA",
                recommendation = "Avaliação otorrinolaringológica e possível tratamento cirúrgico"
            });
        }

        // 3. Mechanical problems (2 diagnoses)
        if (responses.TryGetValue("uso_cotonete", out var respCotonete) && respCotonete != "Não" &&
            responses.TryGetValue("tipo_sintoma", out respTipo) && 
            (respTipo == "Coceira" || respTipo == "Sensação de ouvido tampado"))
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "IMPACTO DE CERA OU LESÃO POR COTONETE",
                recommendation = "Não tente remover - procure limpeza profissional com otorrinolaringologista"
            });
        }

        if (responses.TryGetValue("barotrauma", out var respBarotrauma) && respBarotrauma == "Sim, com dor intensa" &&
            responses.TryGetValue("perda_auditiva", out respPerda) && respPerda != "Sem perda")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "BAROTRAUMA",
                recommendation = "Descongestionantes nasais podem ajudar - evite novos mergulhos/voos até melhora"
            });
        }

        // 4. Neurological/chronic conditions (2 diagnoses)
        if (responses.TryGetValue("zumbido_caracteristica", out var respZumbido) && respZumbido == "Sim" &&
            responses.TryGetValue("pressao_alta", out var respPressao) && respPressao == "Sim")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "ZUMBIDO PULSÁTIL (POSSÍVEL CAUSA VASCULAR)",
                recommendation = "Avaliação cardiológica e otológica - controle rigoroso da pressão arterial"
            });
        }

        if (responses.TryGetValue("tipo_sintoma", out respTipo) && respTipo == "Diminuição da audição" &&
            responses.TryGetValue("exposicao_ruido", out var respRuido) && respRuido != "Não")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "PERDA AUDITIVA INDUZIDA POR RUÍDO",
                recommendation = "Proteção auditiva obrigatória e avaliação audiológica completa"
            });
        }

        // 5. TMJ disorder
        if (responses.TryGetValue("dor_mandibular", out var respMandibula) && respMandibula == "Sim" &&
            responses.TryGetValue("tipo_sintoma", out respTipo) && respTipo == "Dor")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "DISFUNÇÃO DA ARTICULAÇÃO TEMPOROMANDIBULAR (ATM)",
                recommendation = "Avaliação com dentista especializado em ATM"
            });
        }

        // 6. Ototoxicity
        if (responses.TryGetValue("uso_medicamentos", out var respMedicamentos) && respMedicamentos != "Não" &&
            responses.TryGetValue("perda_auditiva", out respPerda) && respPerda != "Sem perda")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "OTOTOXICIDADE MEDICAMENTOSA",
                recommendation = "Interrompa medicação se possível e consulte médico prescritor"
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
        if (totalScore >= 50) return "🚨 RISCO MUITO ELEVADO - Procure ajuda médica IMEDIATA";
        if (totalScore >= 30) return "⚠️ RISCO MODERADO/ALTO - Agende avaliação em até 24h";
        if (totalScore >= 15) return "🔍 RISCO LEVE - Monitore sintomas e consulte se persistirem";
        return "✅ BAIXO RISCO - Mantenha cuidados auditivos";
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
        string path = Path.Combine(Application.persistentDataPath, "diagnostico_ears.json");
        File.WriteAllText(path, json);
        Debug.Log("Resultados salvos em: " + path);
    }
}