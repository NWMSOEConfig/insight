describe('Example Read & Click Test', () => {
  it('Visits the configuration page', () => {
    cy.visit('/')
    cy.contains('Category A')
    cy.get('button').contains('Category A').click()
    cy.contains('Subcategory 1')
  })
})
