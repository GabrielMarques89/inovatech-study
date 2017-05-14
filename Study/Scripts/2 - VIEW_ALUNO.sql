CREATE OR REPLACE VIEW VW_ALUNO AS SELECT
    ALUN.ID_ALUNO AS ID_ALUNO,
    ALUN.MATRICULA AS MATRICULA,
    ALUN.NOME AS NOME,
    ALUN.EMAIL AS EMAIL,
    ALUN.TELEFONE AS TELEFONE,
    ALUN.ID_CURSO AS ID_CURSO,
    CURS.NOME AS NOME_CURSO,
    ALUN.PERIODO AS PERIODO,
    ALUN.FOTO_URL AS FOTO_URL,
    ALUN.FOTO_THUMB_URL AS FOTO_THUMB_URL,
    (SELECT count(*) FROM AVALIACOES AVAL
     WHERE AVAL.ID_AVALIADO = ALUN.ID_ALUNO AND AVAL.AVALIACAO_POSITIVA = TRUE) AS INDICACOES,
    (SELECT count(*) FROM AVALIACOES AVAL
     WHERE AVAL.ID_AVALIADO = ALUN.ID_ALUNO AND AVAL.AVALIACAO_POSITIVA = FALSE) AS CONTRA_INDICACOES,
	ALUN.VERSION AS VERSION
FROM ALUNOS ALUN
INNER JOIN CURSOS CURS ON ALUN.ID_CURSO = CURS.ID_CURSO
LEFT JOIN PARTICIPACOES PART ON PART.ID_ALUNO = ALUN.ID_ALUNO
LEFT JOIN GRUPOS_ESTUDO GRUP ON GRUP.ID_GRUPO_ESTUDO = PART.ID_GRUPO_ESTUDO
LEFT JOIN AVALIACOES AVAL ON AVAL.ID_AVALIADO = ALUN.ID_ALUNO
                          AND AVAL.ID_GRUPO_ESTUDO = GRUP.ID_GRUPO_ESTUDO
GROUP BY ALUN.ID_ALUNO;
