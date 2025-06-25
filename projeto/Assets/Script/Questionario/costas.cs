using System;
using System.Collections.Generic;
using UnityEngine;

public class BackQuiz : MonoBehaviour
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

    private List<Question> backQuestions = new List<Question>();
    private int currentQuestionIndex = 0;

    private void Start()
    {
        InitializeBackQuestions();
    }

    private void InitializeBackQuestions()
    {
        backQuestions = new List<Question>
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
                    {"A", ("Região lombar (parte baixa)", 4)},
                    {"B", ("Região dorsal (meio das costas)", 3)},
                    {"C", ("Cervical (pescoço)", 3)},
                    {"D", ("Dor generalizada", 2)},
                    {"E", ("Em um ponto específico", 3)}
                }
            },
            new Question
            {
                id = "duracao",
                question = "3. Há quanto tempo você está com dor?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Menos de 1 dia", 1)},
                    {"B", ("1 a 7 dias", 3)},
                    {"C", ("1 a 4 semanas", 5)},
                    {"D", ("Mais de 1 mês", 7)}
                }
            },
            new Question
            {
                id = "irradiacao",
                question = "4. A dor irradia para outras áreas?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, para pernas/glúteos", 5)},
                    {"B", ("Sim, para braços/ombros", 5)},
                    {"C", ("Não irradia", 0)}
                }
            },
            new Question
            {
                id = "formigamento",
                question = "5. Você sente formigamento ou dormência?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, constante", 6)},
                    {"B", ("Sim, ocasional", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "piora_movimento",
                question = "6. A dor piora com movimento ou esforço?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, significativamente", 5)},
                    {"B", ("Sim, levemente", 2)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "trauma",
                question = "7. Você teve algum trauma ou levantou peso recentemente?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, trauma direto", 8)},
                    {"B", ("Sim, esforço físico", 5)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "febre",
                question = "8. Você tem febre?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, acima de 38°C", 7)},
                    {"B", ("Sim, abaixo de 38°C", 4)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "perda_peso",
                question = "9. Você teve perda de peso sem motivo?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, mais de 5kg", 6)},
                    {"B", ("Sim, menos de 5kg", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "urinario_intestinal",
                question = "10. Você tem dificuldade para controlar a urina ou evacuar?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, ambos", 15)},
                    {"B", ("Sim, apenas urina", 10)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "rigidez_matinal",
                question = "11. Você sente rigidez nas costas pela manhã?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, dura mais de 1 hora", 5)},
                    {"B", ("Sim, dura menos de 1 hora", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "melhora_repouso",
                question = "12. A dor melhora com repouso?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, completamente", 1)},
                    {"B", ("Parcialmente", 3)},
                    {"C", ("Não melhora", 5)}
                }
            },
            new Question
            {
                id = "postura",
                question = "13. Você trabalha em posição sentada por longos períodos?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, mais de 6h/dia", 4)},
                    {"B", ("Sim, 3-6h/dia", 2)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "historia_artrite",
                question = "14. Há histórico de artrite ou problemas reumáticos na família?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim", 4)},
                    {"B", ("Não sei", 1)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "osteoporose",
                question = "15. Você tem diagnóstico de osteoporose?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim", 6)},
                    {"B", ("Suspeito que sim", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "medicamentos",
                question = "16. Você usa medicamentos para dor nas costas?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, frequentemente", 5)},
                    {"B", ("Sim, ocasionalmente", 2)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "dormencia_pernas",
                question = "17. Quando sentado, sente dormência nas pernas?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, frequentemente", 5)},
                    {"B", ("Às vezes", 3)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "trajetoria_dor",
                question = "18. Como evoluiu sua dor desde o início?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Piorou progressivamente", 5)},
                    {"B", ("Manteve-se estável", 2)},
                    {"C", ("Melhorou", 0)}
                }
            },
            new Question
            {
                id = "fratura_prev",
                question = "19. Já teve fratura ou cirurgia na coluna?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Sim, fratura", 6)},
                    {"B", ("Sim, cirurgia", 4)},
                    {"C", ("Não", 0)}
                }
            },
            new Question
            {
                id = "idade",
                question = "20. Qual sua faixa etária?",
                options = new Dictionary<string, (string, int)>
                {
                    {"A", ("Menos de 30 anos", 1)},
                    {"B", ("30-50 anos", 3)},
                    {"C", ("Mais de 50 anos", 5)}
                }
            }
        };
    }

    public void StartBackQuiz()
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

        if (currentQuestionIndex >= backQuestions.Count)
        {
            FinishQuiz();
            return;
        }

        Question currentQuestion = backQuestions[currentQuestionIndex];
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
    }

    private void EvaluateDiagnoses()
    {
        diagnoses.Clear();

        // 1. Emergências (prioridade máxima)
        if (responses.TryGetValue("urinario_intestinal", out var respUrinario) && 
            (respUrinario == "Sim, ambos" || respUrinario == "Sim, apenas urina"))
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "SÍNDROME DA CAUDA EQUINA",
                recommendation = "Emergência médica! Procure atendimento IMEDIATO"
            });
        }
        
        if (responses.TryGetValue("febre", out var respFebre) && respFebre != "Não" &&
            responses.TryGetValue("perda_peso", out var respPeso) && respPeso != "Não")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "POSSÍVEL INFECÇÃO OU TUMOR NA COLUNA",
                recommendation = "Avaliação médica URGENTE necessária"
            });
        }

        // 2. Problemas lombares
        if (responses.TryGetValue("localizacao", out var respLocal) && respLocal == "Região lombar (parte baixa)" &&
            responses.TryGetValue("irradiacao", out var respIrradiacao) && respIrradiacao == "Sim, para pernas/glúteos")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "HÉRNIA DE DISCO LOMBAR",
                recommendation = "Avaliação com ortopedista/neurocirurgião"
            });
        }

        // 3. Problemas cervicais
        if (responses.TryGetValue("localizacao", out respLocal) && respLocal == "Cervical (pescoço)" &&
            responses.TryGetValue("formigamento", out var respFormigamento) && respFormigamento != "Não")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "PROBLEMA CERVICAL (HÉRNIA OU ARTROSE)",
                recommendation = "Avaliação com especialista"
            });
        }

        // 4. Artrites/Espondilites
        if (responses.TryGetValue("rigidez_matinal", out var respRigidez) && respRigidez == "Sim, dura mais de 1 hora" &&
            responses.TryGetValue("historia_artrite", out var respArtrite) && respArtrite == "Sim")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "POSSÍVEL ESPONDILITE ANQUILOSANTE",
                recommendation = "Reumatologista pode ajudar"
            });
        }

        // 5. Osteoporose
        if (responses.TryGetValue("osteoporose", out var respOsteoporose) && respOsteoporose != "Não" &&
            responses.TryGetValue("idade", out var respIdade) && respIdade == "Mais de 50 anos" &&
            responses.TryGetValue("intensidade", out var respIntensidade) &&
            (respIntensidade == "Forte (incapacitante)" || respIntensidade == "A pior dor que já senti"))
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "RISCO DE FRATURA POR OSTEOPOROSE",
                recommendation = "Avaliação de densitometria óssea"
            });
        }

        // 6. Lesão muscular
        if (responses.TryGetValue("trauma", out var respTrauma) && respTrauma != "Não" &&
            responses.TryGetValue("piora_movimento", out var respMovimento) && respMovimento != "Não")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "LESÃO MUSCULAR OU DISTENSÃO",
                recommendation = "Repouso e fisioterapia podem ajudar"
            });
        }

        // 7. Problemas posturais
        if (responses.TryGetValue("postura", out var respPostura) && respPostura != "Não" &&
            responses.TryGetValue("melhora_repouso", out var respRepouso) && respRepouso != "Não melhora")
        {
            diagnoses.Add(new Diagnosis
            {
                condition = "DOR POSTURAL",
                recommendation = "Melhore sua ergonomia e faça alongamentos"
            });
        }
    }

    private void DisplayResults()
    {
        // Limpa resultados anteriores
        foreach (Transform child in diagnosesContainer)
        {
            Destroy(child.gameObject);
        }

        if (diagnoses.Count > 0)
        {
            resultText.text = "🔍 DIAGNÓSTICOS IDENTIFICADOS:";
            for (int i = 0; i < diagnoses.Count; i++)
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

        // Classificação por pontuação
        riskLevelText.text = "NÍVEL DE RISCO GERAL:\n";
        if (totalScore >= 60)
        {
            riskLevelText.text += "RISCO MUITO ELEVADO - Procure ajuda profissional IMEDIATA";
        }
        else if (totalScore >= 35)
        {
            riskLevelText.text += "RISCO MODERADO/ALTO - Agende avaliação médica em até 1 semana";
        }
        else if (totalScore >= 15)
        {
            riskLevelText.text += "RISCO LEVE - Monitore sintomas e consulte se persistirem";
        }
        else
        {
            riskLevelText.text += "BAIXO RISCO - Mantenha hábitos saudáveis";
        }

        scoreText.text = $"Pontuação total: {totalScore}/120";

        // Opcional: resumo das respostas pode ser adicionado aqui
    }
}