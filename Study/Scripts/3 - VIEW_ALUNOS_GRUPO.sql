CREATE OR REPLACE VIEW VW_ALUNOS_GRUPO AS SELECT
  ALUN.ID_ALUNO,
  ALUN.MATRICULA,
  ALUN.NOME,
  ALUN.EMAIL,
  ALUN.TELEFONE,
  ALUN.FOTO,
  ALUN.TOKEN AS TOKEN,
  PART.TIPO,
  GRUP.ID_GRUPO_ESTUDO,
  GRUP.NOME AS NOME_GRUPO,
  ALUN.VERSION
FROM ALUNOS ALUN
INNER JOIN PARTICIPACOES PART ON PART.ID_ALUNO = ALUN.ID_ALUNO
INNER JOIN GRUPOS_ESTUDO GRUP ON GRUP.ID_GRUPO_ESTUDO = PART.ID_GRUPO_ESTUDO
GROUP BY ALUN.ID_ALUNO;