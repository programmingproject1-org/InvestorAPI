#!/usr/bin/env python
# -*- coding: utf-8 -*-
from requests import Session
from .request_extensions.registration_request import RegistrationRequest
from .request_extensions.authentication_request import AuthenticationRequest
from .request_extensions.deletion_request import DeletionRequest
from .request_extensions.viewdetails_request import ViewDetailsRequest
from .request_extensions.currentquotes_request import CurrentQuotesRequest

class ApiFacade:
	def __init__(self):
		pass

	@staticmethod
	def register_user(displayName, email, password):
		session = Session()
		request = RegistrationRequest(session, displayName, email, password)
		response = request.get_response()
		session.close()
		return response

	@staticmethod
	def authenticate_user(email, password):
		session = Session()
		request = AuthenticationRequest(session, email, password)
		response = request.get_response()
		session.close()
		return response

	@staticmethod
	def delete_user(token):
		session = Session()
		request = DeletionRequest(session, token)
		response = request.get_response()
		session.close()
		return response

	@staticmethod
	def view_details(token):
		session = Session()
		request = ViewDetailsRequest(session, token)
		response = request.get_response()
		session.close()
		return response

	@staticmethod
	def get_current_quotes(token, symbols):
		session = Session()
		request = CurrentQuotesRequest(session, token, symbols)
		response = request.get_response()
		session.close()
		return response

	# def buy_share(self, symbol, token):
	# 	buy_share = BuyShare(CURRENT_BUY_SHARE_URL, symbol, token)
	# 	return buy_share.get_outcome()

