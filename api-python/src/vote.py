class Vote(object):
	def __init__(self, prenom, choix):
		self._prenom = prenom
		self._choix = choix

	def setPrenom(self, prenom):
		self._prenom = prenom

	def getPrenom(self):
		return self._prenom

	def setChoix(self, choix):
		self._choix = choix

	def getChoix(self):
		return self._choix	
