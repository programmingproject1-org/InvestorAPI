#!/usr/bin/env python
# -*- coding: utf-8 -*-
from requests import Session
from .request_extensions.registration_request import RegistrationRequest
from .request_extensions.authentication_request import AuthenticationRequest
from .request_extensions.deletion_request import DeletionRequest
from .request_extensions.viewdetails_request import ViewDetailsRequest
from .request_extensions.currentquotes_request import CurrentQuotesRequest
from .request_extensions.buyshare_request import BuyShareRequest
from .request_extensions.sellshare_request import SellShareRequest
from .request_extensions.viewwatchlist_request import ViewWatchlistRequest

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

	@staticmethod
	def buy_share(token, account_id, symbol, quantity):
		session = Session()
		request = BuyShareRequest(session, token, account_id, symbol, quantity)
		response = request.get_response()
		session.close()
		return response

	@staticmethod
	def sell_share(token, account_id, symbol, quantity):
		session = Session()
		request = SellShareRequest(session, token, account_id, symbol, quantity)
		response = request.get_response()
		session.close()
		return response

	@staticmethod
	def get_watchlist(token, watchlist_id):
		session = Session()
		request = ViewWatchlistRequest(session, token, watchlist_id)
		response = request.get_response()
		session.close()
		return response