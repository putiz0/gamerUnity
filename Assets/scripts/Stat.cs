using UnityEngine;

public enum Stat 
{
    // Atributos de Sobrevivência
    VidaMaxima,
    VidaAtual,
    Energia,
    Escudo,

    // Atributos de Defesa e Resistência
    DefesaFisica,
    DefesaMagica,
    DefesaDeCorte,
    DefesaDeFogo,
    DefesaDeVeneno,
    DefesaDeGelo,
    DefesaEletrico,
    ResistenciaFisica, // Percentual
    ResistenciaMagica, // Percentual

    // Atributos de Ataque (Dano Base)
    AtaqueFisico,
    AtaqueMagico,
    AtaqueDeFogo,
    AtaqueDeVeneno,
    AtaqueDeGelo,
    AtaqueEletrico,
    AtaqueDeCorte,
    AtaqueVerdadeiro, // Ignora defesa

    // Utilidade e Modificadores de Combate
    VelocidadeDeMovimento,
    DistanciaDoAtaque,
    VelocidadeDeAtaque,
    PerfuracaoFisica, // Percentual
    PerfuracaoMagica, // Percentual
    AcertoCritico,     // 0–1
    DanoCritico,      // multiplicador (ex: 2 = 200%)
    RouboDeVida, // Percentual
    DanoDeVelocidade, // Conversão de velocidade em dano

    //statu que pode ficar
    StunChance,
    CongelamentoChance,
    Fogo,
    Veneno,

    // Bonus percentuais
    BonusAtaqueFisicoPct,
    BonusVelocidadeAtaquePct,
    BonusDistanciaAtaquePct,
    BonusVelocidadeMovimentoPct,
    BonusRouboVidaPct,
    BonusCongelamentoChancePct,
    BonusCuraPct,
    BonusExperienciaPct,

    //tropes
    Ouro,
    Nivel,
    Experiencia,
    
}
