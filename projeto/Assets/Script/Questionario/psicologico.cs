using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PsychologicalQuiz : MonoBehaviour
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

    private List<Question> psychologicalQuestions = new List<Question>();
    private int currentQuestionIndex = 0;

    private void Start()
    {
        InitializePsychologicalQuestions();
    }

    private void InitializePsychologicalQuestions()
    {
        psychologicalQuestions = new List<Question>
        {
            new Question
            {
                id = "intensidade",
                question = "1. Qual a intensidade do desconforto emocional?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Leve (consigo lidar)", 1)},
                    {"B", ("Moderada (atrapalha algumas atividades)", 3)},
                    {"C", ("Forte (dificuldade para funcionar)", 5)}
                }
            },
            new Question
            {
                id = "sintoma_principal",
                question = "2. Qual o principal sintoma?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Ansiedade/nervosismo", 4)},
                    {"B", ("Tristeza/apatia", 5)},
                    {"C", ("Alterações no sono", 3)},
                    {"D", ("Alterações no apetite", 2)},
                    {"E", ("Irritabilidade", 3)},
                    {"F", ("Dificuldade de concentração", 3)},
                    {"G", ("Fadiga extrema", 2)},
                    {"H", ("Compulsões/rituais", 4)},
                    {"I", ("Vozes/alucinações", 6)},
                    {"J", ("Euforia excessiva", 5)},
                    {"K", ("Comportamentos repetitivos", 3)}
                }
            },
            new Question
            {
                id = "duracao",
                question = "3. Há quanto tempo você está com esses sintomas?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Menos de 1 semana", 1)},
                    {"B", ("1 a 4 semanas", 3)},
                    {"C", ("Mais de 1 mês", 5)}
                }
            },
            new Question
            {
                id = "interferencia",
                question = "4. Os sintomas interferem na sua rotina?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Interfere muito (não consigo trabalhar/estudar)", 5)},
                    {"B", ("Interfere um pouco (dificulta algumas tarefas)", 3)},
                    {"C", ("Não interfere", 0)}
                }
            },
            new Question
            {
                id = "pensamentos_negativos",
                question = "5. Com que frequência tem pensamentos negativos?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Diariamente", 5)},
                    {"B", ("Algumas vezes por semana", 3)},
                    {"C", ("Raramente", 1)},
                    {"D", ("Nunca", 0)}
                }
            },
            new Question
            {
                id = "diagnostico_previo",
                question = "6. Já teve diagnóstico psicológico/psiquiátrico antes?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim", 3)},
                    {"B", ("Não", 0)}
                }
            },
            new Question
            {
                id = "evento_estressante",
                question = "7. Passou por algum evento estressante recente?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, grave (perda, trauma)", 5)},
                    {"B", ("Sim, moderado", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "apoio_social",
                question = "8. Como avalia seu apoio social (família/amigos)?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Bom apoio", 0)},
                    {"B", ("Pouco apoio", 3)},
                    {"C", ("Quase nenhum", 5)}
                }
            },
            new Question
            {
                id = "suicidio",
                question = "9. Nos últimos 30 dias, pensou em suicídio ou automutilação?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, com planejamento", 15)},
                    {"B", ("Sim, sem planejamento", 10)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "euforia",
                question = "10. Teve períodos de euforia excessiva seguidos de queda de humor?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim", 5)},
                    {"B", ("Não", 0)}
                }
            },
            new Question
            {
                id = "compulsao",
                question = "11. Realiza comportamentos repetitivos para aliviar ansiedade?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, frequentemente", 4)},
                    {"B", ("Às vezes", 2)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "substancias",
                question = "12. Usa álcool/drogas para lidar com os sintomas?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, diariamente", 5)},
                    {"B", ("Sim, ocasionalmente", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "medo_excessivo",
                question = "13. Tem medos irracionais que evitam atividades normais?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim", 4)},
                    {"B", ("Não", 0)}
                }
            },
            new Question
            {
                id = "sintomas_fisicos",
                question = "14. Quais sintomas físicos acompanham seu estado emocional?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Palpitações/tremores", 3)},
                    {"B", ("Dores sem causa médica", 2)},
                    {"C", ("Náuseas/tonturas", 2)},
                    {"D", ("Nenhum", 0)}
                }
            },
            new Question
            {
                id = "funcionamento",
                question = "15. Como avalia seu funcionamento geral no trabalho/estudo?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Muito prejudicado", 5)},
                    {"B", ("Moderadamente prejudicado", 3)},
                    {"C", ("Normal", 0)}
                }
            },
            new Question
            {
                id = "relacionamentos",
                question = "16. Como são seus relacionamentos íntimos?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Estáveis e saudáveis", 0)},
                    {"B", ("Intensos com altos e baixos", 4)},
                    {"C", ("Distanciamento constante", 3)}
                }
            },
            new Question
            {
                id = "imagem_corporal",
                question = "17. Como você se sente em relação ao seu corpo?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Satisfeito", 0)},
                    {"B", ("Desconforto moderado", 2)},
                    {"C", ("Ódio intenso/medo de engordar", 5)}
                }
            },
            new Question
            {
                id = "infancia",
                question = "18. Na infância, você apresentava:",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Dificuldade de atenção/hiperatividade", 4)},
                    {"B", ("Dificuldade em interações sociais", 3)},
                    {"C", ("Nenhum desses", 0)}
                }
            },
            new Question
            {
                id = "parto_recente",
                question = "19. Teve parto nos últimos 6 meses?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim", 4)},
                    {"B", ("Não", 0)}
                }
            },
            new Question
            {
                id = "rotinas",
                question = "20. Como lida com mudanças na rotina?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Lida bem", 0)},
                    {"B", ("Desconforto moderado", 2)},
                    {"C", ("Crises de ansiedade", 4)}
                }
            }
        };
    }

    public void StartPsychologicalQuiz()
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

        if (currentQuestionIndex >= psychologicalQuestions.Count)
        {
            FinishQuiz();
            return;
        }

        Question currentQuestion = psychologicalQuestions[currentQuestionIndex];
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
        if (responses.TryGetValue("suicidio", out var respSuicidio) && 
            (respSuicidio == "Sim, com planejamento" || respSuicidio == "Sim, sem planejamento"))
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "RISCO DE SUICÍDIO",
                recommendation = "Procure ajuda IMEDIATA (CVV 188 ou CAPS)"
            });
        }
        
        if (responses.TryGetValue("sintoma_principal", out var respSintoma) && respSintoma == "Vozes/alucinações")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "PSICOSE/ESQUIZOFRENIA",
                recommendation = "Avaliação psiquiátrica URGENTE"
            });
        }

        // 2. Mood disorders
        if (responses.TryGetValue("sintoma_principal", out respSintoma) && respSintoma == "Tristeza/apatia" &&
            responses.TryGetValue("duracao", out var respDuracao) && respDuracao == "Mais de 1 mês")
        {
            if (responses.TryGetValue("parto_recente", out var respParto) && respParto == "Sim")
            {
                diagnoses.Add(new Diagnosis
                {
                    condition = "DEPRESSÃO PÓS-PARTO",
                    recommendation = "Prioridade no tratamento"
                });
            }
            else
            {
                diagnoses.Add(new Diagnosis
                {
                    condition = "DEPRESSÃO MAIOR",
                    recommendation = "Psicoterapia + possível medicação"
                });
            }
        }
        
        if ((responses.TryGetValue("euforia", out var respEuforia) && respEuforia == "Sim") || 
            (responses.TryGetValue("sintoma_principal", out respSintoma) && respSintoma == "Euforia excessiva"))
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "TRANSTORNO BIPOLAR",
                recommendation = "Avaliação psiquiátrica essencial"
            });
        }

        // 3. Anxiety disorders
        if (responses.TryGetValue("medo_excessivo", out var respMedo) && respMedo == "Sim")
        {
            if (responses.TryGetValue("sintomas_fisicos", out var respSintomasFisicos) && respSintomasFisicos == "Palpitações/tremores")
            {
                diagnoses.Add(new Diagnosis
                {
                    condition = "TRANSTORNO DE PÂNICO",
                    recommendation = "Técnicas de grounding + terapia"
                });
            }
            else
            {
                diagnoses.Add(new Diagnosis
                {
                    condition = "FOBIAS ESPECÍFICAS",
                    recommendation = "Terapia de exposição gradual"
                });
            }
        }
        
        if (responses.TryGetValue("sintoma_principal", out respSintoma) && respSintoma == "Ansiedade/nervosismo" &&
            responses.TryGetValue("duracao", out respDuracao) && respDuracao == "Mais de 1 mês" &&
            responses.TryGetValue("interferencia", out var respInterferencia) && respInterferencia == "Interfere muito")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "TRANSTORNO DE ANSIEDADE GENERALIZADA",
                recommendation = "Terapia cognitivo-comportamental"
            });
        }

        // 4. OCD
        if ((responses.TryGetValue("compulsao", out var respCompulsao) && 
             (respCompulsao == "Sim, frequentemente" || respCompulsao == "Às vezes")) &&
            (responses.TryGetValue("sintoma_principal", out respSintoma) && 
             (respSintoma == "Compulsões/rituais" || respSintoma == "Ansiedade/nervosismo")))
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "TRANSTORNO OBSESSIVO-COMPULSIVO (TOC)",
                recommendation = "Terapia especializada"
            });
        }

        // 5. Eating disorders
        if (responses.TryGetValue("imagem_corporal", out var respImagemCorporal) && respImagemCorporal == "Ódio intenso/medo de engordar" &&
            responses.TryGetValue("sintoma_principal", out respSintoma) && respSintoma != "Alterações no apetite")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "TRANSTORNO ALIMENTAR",
                recommendation = "Nutricionista + psiquiatra especializado"
            });
        }

        // 6. ADHD (Adult)
        if (responses.TryGetValue("infancia", out var respInfancia) && respInfancia == "Dificuldade de atenção/hiperatividade" &&
            responses.TryGetValue("funcionamento", out var respFuncionamento) && respFuncionamento != "Normal")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "TDAH (ADULTO)",
                recommendation = "Avaliação neuropsicológica"
            });
        }

        // 7. Mild Autism
        if (responses.TryGetValue("infancia", out respInfancia) && respInfancia == "Dificuldade em interações sociais" &&
            responses.TryGetValue("rotinas", out var respRotinas) && 
            (respRotinas == "Desconforto moderado" || respRotinas == "Crises de ansiedade"))
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "AUTISMO LEVE (TEA NÍVEL 1)",
                recommendation = "Avaliação com especialista"
            });
        }

        // 8. Borderline
        if (responses.TryGetValue("relacionamentos", out var respRelacionamentos) && respRelacionamentos == "Intensos com altos e baixos" &&
            totalScore > 25)
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "TRANSTORNO BORDERLINE",
                recommendation = "Terapia dialética comportamental (DBT)"
            });
        }

        // 9. Burnout
        if (responses.TryGetValue("sintoma_principal", out respSintoma) && respSintoma == "Fadiga extrema" &&
            responses.TryGetValue("funcionamento", out respFuncionamento) && respFuncionamento == "Muito prejudicado")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "SÍNDROME DE BURNOUT",
                recommendation = "Afastamento + terapia"
            });
        }

        // 10. PTSD
        if (responses.TryGetValue("evento_estressante", out var respEvento) && respEvento == "Sim, grave (perda, trauma)" &&
            responses.TryGetValue("pensamentos_negativos", out var respPensamentos) && 
            (respPensamentos == "Diariamente" || respPensamentos == "Algumas vezes por semana"))
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "TRANSTORNO DE ESTRESSE PÓS-TRAUMÁTICO",
                recommendation = "EMDR ou terapia traumática"
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
            resultText.text = "🟢 Nenhum transtorno específico identificado";
        }

        riskLevelText.text = "NÍVEL DE RISCO GERAL:\n" + GetRiskLevelText();
        scoreText.text = $"Pontuação total: {totalScore}/100";
    }

    private string GetRiskLevelText()
    {
        if (totalScore >= 40) return "🚨 RISCO MUITO ELEVADO - Procure ajuda profissional IMEDIATA";
        if (totalScore >= 25) return "⚠️ RISCO MODERADO/ALTO - Agende avaliação em até 1 semana";
        if (totalScore >= 15) return "🔍 RISCO LEVE - Pratique autocuidado e monitore sintomas";
        return "✅ BAIXO RISCO - Mantenha hábitos saudáveis";
    }

    private string GetRiskLevel()
    {
        if (totalScore >= 40) return "MUITO ELEVADO";
        if (totalScore >= 25) return "ALTO";
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
        string path = Path.Combine(Application.persistentDataPath, "diagnostico_psicologico.json");
        File.WriteAllText(path, json);
        Debug.Log("Resultados salvos em: " + path);
    }
}