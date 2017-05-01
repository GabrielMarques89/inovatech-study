create table STUDY_DB.ALUNOS
(
    ID_ALUNO bigint not null
        primary key,
    VERSION bigint null,
    MATRICULA varchar(128) not null,
    SENHA varchar(128) not null,
    NOME varchar(128) not null,
    EMAIL varchar(128) not null,
    TELEFONE varchar(14) null,
    FOTO_URL text null,
    FOTO_THUMB_URL text null,
    PERIODO int not null,
    TOKEN text null,
    ID_CURSO bigint not null,
    constraint FK_CURSO_X_ALUNO
        foreign key (ID_CURSO) references study_db.CURSOS (ID_CURSO)
);

create index ID_CURSO on ALUNOS (ID_CURSO);

create table STUDY_DB.AVALIACOES
(
    ID_AVALIACAO bigint not null
        primary key,
    VERSION bigint null,
    TEXTO text not null,
    AVALIACAO_POSITIVA tinyint(1) not null,
    ID_AVALIADOR bigint not null,
    ID_AVALIADO bigint not null,
    ID_GRUPO_ESTUDO bigint not null,
    constraint FK_ALUNO_X_AVALIACAO_01
        foreign key (ID_AVALIADOR) references study_db.ALUNOS (ID_ALUNO),
    constraint FK_ALUNO_X_AVALIACAO_02
        foreign key (ID_AVALIADO) references study_db.ALUNOS (ID_ALUNO),
    constraint FK_GRUPO_X_AVALIACAO_01
        foreign key (ID_GRUPO_ESTUDO) references study_db.GRUPOS_ESTUDO (ID_GRUPO_ESTUDO)
);

create index ID_AVALIADO on AVALIACOES (ID_AVALIADO);

create index ID_AVALIADOR on AVALIACOES (ID_AVALIADOR);

create index ID_GRUPO_ESTUDO on AVALIACOES (ID_GRUPO_ESTUDO);

create table STUDY_DB.CURSOS
(
    ID_CURSO bigint not null
        primary key,
    VERSION bigint null,
    NOME varchar(128) not null,
    QUANTIDADE_PERIODOS int not null,
    ID_INSTITUICAO bigint not null,
    constraint FK_INSTITUICAO_X_CURSO
        foreign key (ID_INSTITUICAO) references study_db.INSTITUICOES (ID_INSTITUICAO)
);

create index ID_INSTITUICAO on CURSOS (ID_INSTITUICAO);

create table STUDY_DB.DISCIPLINAS
(
    ID_DISCIPLINA bigint not null
        primary key,
    VERSION bigint null,
    NOME varchar(128) not null,
    PROFESSOR varchar(128) not null,
    ID_CURSO bigint not null,
    constraint FK_CURSO_X_DISCIPLINA
        foreign key (ID_CURSO) references study_db.CURSOS (ID_CURSO)
);

create index ID_CURSO on DISCIPLINAS (ID_CURSO);

create table STUDY_DB.GRUPOS_ESTUDO
(
    ID_GRUPO_ESTUDO bigint not null
        primary key,
    VERSION bigint null,
    NOME varchar(128) not null,
    QUANTIDADE_MAX_ALUNOS int not null,
    PRIVADO tinyint(1) not null,
    ID_DISCIPLINA bigint not null,
    constraint FK_DISCIPLINA_X_GRUPO_ESTUDO
        foreign key (ID_DISCIPLINA) references study_db.DISCIPLINAS (ID_DISCIPLINA)
);

create index ID_DISCIPLINA on GRUPOS_ESTUDO (ID_DISCIPLINA);

create table STUDY_DB.INSTITUICOES
(
    ID_INSTITUICAO bigint not null
        primary key,
    VERSION bigint null,
    NOME varchar(128) not null,
    ENDERECO text null,
    TELEFONE varchar(14) null
);

create table STUDY_DB.PARTICIPACOES
(
    ID_PARTICIPACAO bigint not null
        primary key,
    VERSION bigint null,
    TIPO int not null,
    ID_ALUNO bigint not null,
    ID_GRUPO_ESTUDO bigint not null,
    constraint FK_ALUNO_X_PARTICIPACAO
        foreign key (ID_ALUNO) references study_db.ALUNOS (ID_ALUNO),
    constraint FK_GRUPO_X_PARTICIPACAO
        foreign key (ID_GRUPO_ESTUDO) references study_db.GRUPOS_ESTUDO (ID_GRUPO_ESTUDO)
);

create index ID_ALUNO on PARTICIPACOES (ID_ALUNO);

create index ID_GRUPO_ESTUDO on PARTICIPACOES (ID_GRUPO_ESTUDO);