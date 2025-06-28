using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StomachQuiz : MonoBehaviour
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

    private List<Question> stomachQuestions = new List<Question>();
    private int currentQuestionIndex = 0;

    private void Start()
    {
        InitializeStomachQuestions();
    }

    private void InitializeStomachQuestions()
    {
        stomachQuestions = new List<Question>
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
                    {"A", ("Epigástrio (boca do estômago)", 4)},
                    {"B", ("Lado direito superior", 3)},
                    {"C", ("Lado esquerdo superior", 3)},
                    {"D", ("Difusa no abdômen", 2)}
                }
            },
            new Question
            {
                id = "caracteristica",
                question = "3. Como descreveria a dor?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Queimação", 4)},
                    {"B", ("Pontada", 3)},
                    {"C", ("Cólica", 3)},
                    {"D", ("Peso/plenitude", 2)}
                }
            },
            new Question
            {
                id = "duracao",
                question = "4. Há quanto tempo você está com dor?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Menos de 1 dia", 1)},
                    {"B", ("1-3 dias", 3)},
                    {"C", ("4-7 dias", 5)},
                    {"D", ("Mais de 1 semana", 7)}
                }
            },
            new Question
            {
                id = "relacao_alimentar",
                question = "5. A dor está relacionada à alimentação?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Piora após comer", 4)},
                    {"B", ("Melhora após comer", 3)},
                    {"C", ("Sem relação clara", 1)}
                }
            },
            new Question
            {
                id = "nausea",
                question = "6. Você sente náuseas?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, com vômitos", 5)},
                    {"B", ("Sim, sem vômitos", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "azia",
                question = "7. Você tem azia ou regurgitação ácida?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, frequente", 5)},
                    {"B", ("Sim, ocasional", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "apetite",
                question = "8. Como está seu apetite?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Normal", 0)},
                    {"B", ("Reduzido", 3)},
                    {"C", ("Nenhum", 5)}
                }
            },
            new Question
            {
                id = "antiacidos",
                question = "9. A dor melhora com antiácidos?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, completamente", 2)},
                    {"B", ("Sim, parcialmente", 4)},
                    {"C", ("Não", 6)}
                }
            },
            new Question
            {
                id = "febre",
                question = "10. Você tem febre?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, acima 38°C", 5)},
                    {"B", ("Sim, até 38°C", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "fezes",
                question = "11. Como estão suas fezes?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Normais", 0)},
                    {"B", ("Escuras/alcatroadas", 8)},
                    {"C", ("Com sangue visível", 10)},
                    {"D", ("Diarreia", 3)}
                }
            },
            new Question
            {
                id = "perda_peso",
                question = "12. Você teve perda de peso não intencional?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, mais de 5kg", 7)},
                    {"B", ("Sim, menos de 5kg", 4)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "ingestao_alcool",
                question = "13. Você consome álcool frequentemente?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, diariamente", 6)},
                    {"B", ("Sim, ocasionalmente", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "medicamentos",
                question = "14. Você usa anti-inflamatórios frequentemente?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, diariamente", 6)},
                    {"B", ("Sim, ocasionalmente", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "historia_ulcera",
                question = "15. Você tem histórico de úlcera ou gastrite?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, úlcera", 6)},
                    {"B", ("Sim, gastrite", 4)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "ictericia",
                question = "16. Você notou pele ou olhos amarelados?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim", 8)},
                    {"B", ("Não", 0)}
                }
            },
            new Question
            {
                id = "distensao",
                question = "17. Você sente distensão abdominal?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, intensa", 4)},
                    {"B", ("Sim, leve", 2)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "vomito_sangue",
                question = "18. Você vomitou sangue ou material escuro?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim", 15)},
                    {"B", ("Não", 0)}
                }
            },
            new Question
            {
                id = "alergias",
                question = "19. Você tem alergias alimentares conhecidas?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim", 3)},
                    {"B", ("Não", 0)}
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

    public void StartStomachQuiz()
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

        if (currentQuestionIndex >= stomachQuestions.Count)
        {
            FinishQuiz();
            return;
        }

        Question currentQuestion = stomachQuestions[currentQuestionIndex];
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
        if ((responses.TryGetValue("vomito_sangue", out var respVomito) && respVomito == "Sim") ||
            (responses.TryGetValue("fezes", out var respFezes) && 
             (respFezes == "Escuras/alcatroadas" || respFezes == "Com sangue visível")))
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "HEMORRAGIA DIGESTIVA",
                recommendation = "Emergência médica! Procure atendimento IMEDIATAMENTE"
            });
        }

        if (responses.TryGetValue("ictericia", out var respIctericia) && respIctericia == "Sim" &&
            responses.TryGetValue("localizacao", out var respLocalizacao) && respLocalizacao == "Lado direito superior")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "POSSÍVEL PROBLEMA HEPÁTICO OU BILIAR",
                recommendation = "Avaliação médica urgente necessária"
            });
        }

        // 2. Úlcera gástrica
        if (responses.TryGetValue("historia_ulcera", out var respUlcera) && respUlcera != "Não" &&
            responses.TryGetValue("antiacidos", out var respAntiacidos) && respAntiacidos != "Não" &&
            responses.TryGetValue("medicamentos", out var respMedicamentos) && respMedicamentos != "Não")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "ÚLCERA PÉPTICA",
                recommendation = "Pode requerer endoscopia e tratamento específico"
            });
        }

        // 3. Gastrite
        if (responses.TryGetValue("caracteristica", out var respCaracteristica) && respCaracteristica == "Queimação" &&
            responses.TryGetValue("azia", out var respAzia) && respAzia != "Não" &&
            responses.TryGetValue("relacao_alimentar", out var respAlimentar) && respAlimentar == "Piora após comer")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "GASTRITE",
                recommendation = "Evite alimentos irritantes e use antiácidos"
            });
        }

        // 4. Refluxo gastroesofágico
        if (responses.TryGetValue("azia", out respAzia) && respAzia == "Sim, frequente" &&
            responses.TryGetValue("relacao_alimentar", out respAlimentar) && respAlimentar == "Piora após comer" &&
            responses.TryGetValue("nausea", out var respNausea) && respNausea != "Sim, com vômitos")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "REFLUXO GASTROESOFÁGICO",
                recommendation = "Eleve a cabeceira da cama e evite deitar após comer"
            });
        }

        // 5. Colecistite
        if (responses.TryGetValue("localizacao", out respLocalizacao) && respLocalizacao == "Lado direito superior" &&
            responses.TryGetValue("febre", out var respFebre) && respFebre != "Não" &&
            responses.TryGetValue("relacao_alimentar", out respAlimentar) && respAlimentar == "Piora após comer")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "POSSÍVEL COLECISTITE",
                recommendation = "Avaliação médica e ultrassom necessários"
            });
        }

        // 6. Pancreatite
        if (responses.TryGetValue("intensidade", out var respIntensidade) && 
            (respIntensidade == "Forte (incapacitante)" || respIntensidade == "A pior dor que já senti") &&
            responses.TryGetValue("localizacao", out respLocalizacao) && respLocalizacao == "Epigástrio (boca do estômago)" &&
            responses.TryGetValue("vomito_sangue", out respVomito) && respVomito == "Sim")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "POSSÍVEL PANCREATITE",
                recommendation = "Emergência médica - requer avaliação imediata"
            });
        }

        // 7. Dispepsia funcional
        if (responses.TryGetValue("estresse", out var respEstresse) && respEstresse != "Não" &&
            responses.TryGetValue("caracteristica", out respCaracteristica) && 
            (respCaracteristica == "Peso/plenitude" || respCaracteristica == "Cólica") &&
            responses.TryGetValue("antiacidos", out respAntiacidos) && respAntiacidos == "Não")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "DISPEPSIA FUNCIONAL",
                recommendation = "Pode estar relacionada ao estresse - técnicas de relaxamento podem ajudar"
            });
        }

        // 8. Intolerância alimentar
        if (responses.TryGetValue("alergias", out var respAlergias) && respAlergias == "Sim" &&
            responses.TryGetValue("relacao_alimentar", out respAlimentar) && respAlimentar == "Piora após comer" &&
            responses.TryGetValue("distensao", out var respDistensao) && respDistensao != "Não")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "INTOLERÂNCIA ALIMENTAR",
                recommendation = "Identifique e evite alimentos desencadeantes"
            });
        }

        // 9. Síndrome do intestino irritável
        if (responses.TryGetValue("caracteristica", out respCaracteristica) && respCaracteristica == "Cólica" &&
            responses.TryGetValue("fezes", out respFezes) && respFezes == "Diarreia" &&
            responses.TryGetValue("estresse", out respEstresse) && respEstresse != "Não")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "SÍNDROME DO INTESTINO IRRITÁVEL",
                recommendation = "Manejo do estresse e ajuste dietético podem ajudar"
            });
        }

        // 10. Câncer gástrico (para sintomas persistentes)
        if (responses.TryGetValue("perda_peso", out var respPerdaPeso) && respPerdaPeso != "Não" &&
            responses.TryGetValue("apetite", out var respApetite) && respApetite != "Normal" &&
            responses.TryGetValue("duracao", out var respDuracao) && respDuracao == "Mais de 1 semana")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "RISCO DE CÂNCER GÁSTRICO",
                recommendation = "Avaliação gastroenterológica urgente"
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
        string path = Path.Combine(Application.persistentDataPath, "diagnostico_stomach.json");
        File.WriteAllText(path, json);
        Debug.Log("Resultados salvos em: " + path);
    }
}