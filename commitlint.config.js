export default {
  extends: ['@commitlint/config-conventional'],
  rules: {
    'type-enum': [
      2,
      'always',
      [
        'feat',     // nueva funcionalidad
        'fix',      // corrección de bug
        'docs',     // documentación
        'style',    // formato, espacios (sin cambio lógico)
        'refactor', // refactoring
        'test',     // pruebas
        'chore',    // configuración, dependencias
        'perf',     // mejora de rendimiento
        'ci',       // cambios en CI/CD
        'revert',   // revertir commit
      ],
    ],
    'subject-case': [2, 'always', 'lower-case'],
    'subject-max-length': [2, 'always', 72],
  },
}
