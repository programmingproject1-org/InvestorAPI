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
from .request_extensions.addtowatchlist_request import AddToWatchlistRequest
from .request_extensions.removefromwatchlist_request import RemoveFromWatchlistRequest
from .request_extensions.historicalprices_request import HistoricalPricesRequest
from .request_extensions.viewportfolio_request import ViewPortfolioRequest
from .request_extensions.viewtransactions_request import ViewTransactionsRequest
from .request_extensions.leaderboard_request import LeaderboardRequest

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

	@staticmethod
	def add_to_watchlist(token, watchlist_id, symbol):
		session = Session()
		request = AddToWatchlistRequest(session, token, watchlist_id, symbol)
		response = request.get_response()
		session.close()
		return response

	@staticmethod
	def remove_from_watchlist(token, watchlist_id, symbol):
		session = Session()
		request = RemoveFromWatchlistRequest(session, token, watchlist_id, symbol)
		response = request.get_response()
		session.close()
		return response

	@staticmethod
	def get_historical_prices(token, symbol, end_time = None,
		interval = None, date_range = None):
		session = Session()
		request = HistoricalPricesRequest(session, token, symbol, end_time, 
			interval, date_range)
		response = request.get_response()
		session.close()
		return response

	@staticmethod
	def get_portfolio(token, account_id):
		session = Session()
		request = ViewPortfolioRequest(session, token, account_id)
		response = request.get_response()
		session.close()
		return response

	@staticmethod
	def get_transactions(token, account_id, page_number = None, 
		page_size = None, start_date = None, end_date = None):
		session = Session()
		request = ViewTransactionsRequest(session, token, account_id, page_number, 
			page_size, start_date, end_date)
		response = request.get_response()
		session.close()
		return response

	@staticmethod
	def get_leaderboard(token, page_number = None, page_size = None):
		session = Session()
		request = LeaderboardRequest(session, token, page_number, page_size)
		response = request.get_response()
		session.close()
		return response